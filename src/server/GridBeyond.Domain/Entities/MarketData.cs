using System;

namespace GridBeyond.Domain.Entities
{
    public class MarketData
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double MarketPriceEX1 { get; set; }
    }
}