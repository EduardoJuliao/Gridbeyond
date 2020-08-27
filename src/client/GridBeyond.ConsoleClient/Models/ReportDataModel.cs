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
        public PeakQuiet HighestPeak { get; set; }
        public PeakQuiet QuieterHour { get; set; }
        public PeakQuiet[] PeakQuietPerDate { get; set; }
    }

    public class PeakQuiet
    {
        public DateTime Date { get; set; }
        public DateTime[] PeakHours { get; set; }
        public DateTime[] QuietHours { get; set; }
    }
}
