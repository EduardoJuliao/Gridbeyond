using System;
using System.Collections.Generic;
using System.Linq;

namespace GridBeyond.Domain.Models
{
    public class ReportData
    {
        public DateTime? HighestValueDate { get; set; }
        public double HighestValue { get; set; }
        public DateTime? LowestValueDate { get; set; }
        public double LowestValue { get; set; }
        public double AverageValue { get; set; }
        public int TotalRecords { get; set; }
        public List<PeakQuiet> PeakQuietPerDate { get; set; }
        public PeakQuiet HighestPeak => HighestValueDate.HasValue
            ? PeakQuietPerDate.Single(x => x.Date.Date == HighestValueDate.Value.Date)
            : null;
        public PeakQuiet QuieterHour => LowestValueDate.HasValue
            ? PeakQuietPerDate.Single(x => x.Date.Date == LowestValueDate.Value.Date)
            : null;
    }

    public class PeakQuiet
    {
        public DateTime Date { get; set; }
        public DateTime[] PeakHours { get; set; }
        public DateTime[] QuietHours { get; set; }
    }
}