using System;
using System.Collections.Generic;
using Cognizant.TickerDataApp.Domain.ValueObjects;

namespace Cognizant.TickerDataApp.Application.Services
{
    public interface ITickersSourceService
    {
        public ICollection<HistoryRecord> GetTickerInfo(string tickerName, DateTime dateFrom, DateTime dateTo);
    }
}