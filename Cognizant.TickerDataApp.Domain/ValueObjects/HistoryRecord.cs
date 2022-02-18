using System;

namespace Cognizant.TickerDataApp.Domain.ValueObjects
{
    public class HistoryRecord
    {
        public HistoryRecord(DateTime date, decimal value)
        {
            Date = date;
            Value = value;
        }

        public DateTime Date { get; }

        public decimal Value { get; }
    }
}