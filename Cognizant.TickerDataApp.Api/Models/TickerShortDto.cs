using System;

namespace Cognizant.TickerDataApp.Api.Models
{
    public class TickerShortDto
    {
        public TickerShortDto(string id, string code, DateTime dateFrom, DateTime dateTo)
        {
            Id = id;
            Code = code;
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public string Id { get; }

        public string Code { get; }
        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }
    }
}