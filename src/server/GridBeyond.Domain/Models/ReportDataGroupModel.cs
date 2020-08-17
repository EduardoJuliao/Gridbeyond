using System;

namespace GridBeyond.Domain.Models
{
    internal class ReportDataGroupModel
    {
        public DateTime Date { get; set; }
        public double Average { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
    }
}