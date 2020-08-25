using System;

namespace GridBeyond.Domain.Models
{
    public class ReportDataGroupModel
    {
        public DateTime Date { get; set; }
        public double Average { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}