using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Application.Services;
using Company.TickerDataApp.Domain.ValueObjects;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Company.TickerDataApp.GoogleSheetsSource
{
    public class GoogleSheetsTickersSource : ITickersSourceService
    {
        //TODO move to appsettings
        private const string ApplicationName = "TickersApp";
        private const string SpreadsheetId = "1zmfdqsvEamBNmbzpSFWxjK_q06yumbD9b7KUqBkjB3s";

        private readonly SheetsService _service;
        private const string StartDataCell = "A1";

        public GoogleSheetsTickersSource(IOptions<GoogleApiAuthSettings> settings)
        {
            var json = JsonSerializer.Serialize(settings.Value);
            var credentials = GoogleCredential.FromJson(json).CreateScoped(SheetsService.Scope.Spreadsheets);

            _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer 
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName
            });
        }
        public async Task<ICollection<HistoryRecord>> GetTickerInfo(string tickerName, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken)
        {
            var (sheetName, sheetId) = await CreateSheet(tickerName, dateFrom, dateTo, cancellationToken);
            
            await Calculate(tickerName, dateFrom, dateTo, sheetName, cancellationToken);

            var historyRecords = await ReadCalculatedData(sheetName, dateFrom, dateTo, cancellationToken);

            await DeleteSheet(sheetId, cancellationToken);
            
            return historyRecords;
        }

        private Task DeleteSheet(int sheetId, CancellationToken cancellationToken)
        {
            var deleteSheetRequest = new DeleteSheetRequest
            {
                SheetId = sheetId
            };

            var batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { new() { DeleteSheet = deleteSheetRequest } }
            };

            var request = _service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, SpreadsheetId);

            return request.ExecuteAsync(cancellationToken);
        }

        private async Task<ICollection<HistoryRecord>> ReadCalculatedData(string sheetName, DateTime dateFrom,
            DateTime dateTo, CancellationToken cancellationToken)
        {
            var range = $"{sheetName}!A2:B{(dateTo - dateFrom).Days + 1}";

            var request = _service.Spreadsheets.Values.Get(SpreadsheetId, range);

            var response = await request.ExecuteAsync(cancellationToken);

            return response.Values.Select(v => new HistoryRecord(DateTime.Parse(v[0].ToString(), CultureInfo.InvariantCulture), decimal.Parse(v[1].ToString(), CultureInfo.InvariantCulture))).ToList();
        }

        private async Task Calculate(string tickerName, DateTime dateFrom, DateTime dateTo, string sheetName, CancellationToken cancellationToken)
        {
            var range = $"{sheetName}!{StartDataCell}";
            var value =
                $"=GOOGLEFINANCE(\"{tickerName}\"; \"price\"; ДАТА({dateFrom.Year};{dateFrom.Month};{dateFrom.Day}); ДАТА({dateTo.Year};{dateTo.Month};{dateTo.Day}); \"DAILY\")";
                var valueRange = new ValueRange { Values = new List<IList<object>>() { new List<object> { value } } };


            var request = _service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            request.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

            var response = await request.ExecuteAsync(cancellationToken);
        }

        private async Task<(string, int)> CreateSheet(string tickerName, DateTime dateFrom, DateTime dateTo,
            CancellationToken cancellationToken)
        {
            var addSheetRequest = new AddSheetRequest
            {
                Properties = new SheetProperties
                {
                    Title = $"{tickerName}-{dateFrom.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)}-{dateTo.ToString(CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern)}"
                }
            };


            var batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> {new() {AddSheet = addSheetRequest}}
            };

            var request = _service.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, SpreadsheetId);

            var response = await request.ExecuteAsync(cancellationToken);

            return (addSheetRequest.Properties.Title, response.Replies.First().AddSheet.Properties.SheetId.Value);
        }
    }
}
