using System;

namespace GridBeyond.Domain.Models
{
    public class ReportData
    {
        public DateTime HighestValueDate { get; set; }
        public double HighestValue { get; set; }
        public DateTime LowestValueDate { get; set; }
        public double LowestValue { get; set; }
        public double AvarageValue { get; set; }
    }
}