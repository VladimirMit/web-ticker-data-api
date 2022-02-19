using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Domain.ValueObjects;

namespace Cognizant.TickerDataApp.Application.Services
{
    public interface ITickersSourceService
    {
        public Task<ICollection<HistoryRecord>> GetTickerInfo(string tickerName, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken);
    }
}