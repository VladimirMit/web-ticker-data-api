using System.Collections.Generic;
using Cognizant.TickerDataApp.Domain.ValueObjects;

namespace Cognizant.TickerDataApp.Domain.Entities
{
    public class Ticker : IEntity
    {
        public Ticker(string id, string code, ICollection<HistoryRecord> historyRecords)
        {
            Id = id;
            Code = code;
            HistoryRecords = historyRecords;
        }

        public string Id { get; }
        public string Code { get; }
        public ICollection<HistoryRecord> HistoryRecords { get; }
    }
}
