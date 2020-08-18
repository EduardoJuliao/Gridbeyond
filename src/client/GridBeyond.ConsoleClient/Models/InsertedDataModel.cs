using System;
using System.Collections.Generic;

namespace GridBeyond.ConsoleClient.Models
{
    public class InsertDataModel
    {
        public DateTime Date { get; set; }
        public double MarketPriceEX1 { get; set; }
    }

    public class InsertedDataModel
    {
        public List<InsertDataModel> ValidRecords { get; set; }
        public List<int> InvalidRecords { get; set; }
        public List<InsertDataModel> NewRecords { get; set; }
    }
}
