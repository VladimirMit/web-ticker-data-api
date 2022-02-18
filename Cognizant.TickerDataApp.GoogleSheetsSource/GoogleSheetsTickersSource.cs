using System;
using System.Collections.Generic;
using System.IO;
using Cognizant.TickerDataApp.Application.Services;
using Cognizant.TickerDataApp.Domain.ValueObjects;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;

namespace Cognizant.TickerDataApp.GoogleSheetsSource
{
    public class GoogleSheetsTickersSource : ITickersSourceService
    {
        //TODO move to appsettings
        private const string ApplicationName = "TickersApp";
        private const string SpreadsheetId = "1zmfdqsvEamBNmbzpSFWxjK_q06yumbD9b7KUqBkjB3s";
        private const string Sheet = "Tickers";
        private const string CredentialsFileName = "cognizant-tickers-data-app-30232472dba3.json";
        
        private readonly SheetsService _service;
        
        public GoogleSheetsTickersSource()
        {
            GoogleCredential credentials;
            
            using (var stream = new FileStream(CredentialsFileName, FileMode.Open, FileAccess.Read))
            {
                credentials = GoogleCredential.FromStream(stream).CreateScoped(new[] { SheetsService.Scope.Spreadsheets });
            } 
                
            _service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer 
            {
                HttpClientInitializer = credentials,
                ApplicationName = ApplicationName
            });
        }
        public ICollection<HistoryRecord> GetTickerInfo(string tickerName, DateTime dateFrom, DateTime dateTo)
        {
            throw new NotImplementedException();
        }
    }
}
