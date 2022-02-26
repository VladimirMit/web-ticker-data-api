using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Domain.ValueObjects;

namespace Company.TickerDataApp.Application.Services
{
    public interface ITickersSourceService
    {
        public Task<ICollection<HistoryRecord>> GetTickerInfo(string tickerName, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken);
    }
}