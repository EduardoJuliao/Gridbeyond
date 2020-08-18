using System;
namespace GridBeyond.ConsoleClient.Models
{
    public class ReportDataModel
    {
        public DateTime HighestValueDate { get; set; }
        public double HighestValue { get; set; }
        public DateTime LowestValueDate { get; set; }
        public double LowestValue { get; set; }
        public double AverageValue { get; set; }
        public int TotalRecords { get; set; }
    }
}
