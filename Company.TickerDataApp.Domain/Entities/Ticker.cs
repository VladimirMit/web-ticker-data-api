using System.Collections.Generic;
using Company.TickerDataApp.Domain.ValueObjects;

namespace Company.TickerDataApp.Domain.Entities
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
