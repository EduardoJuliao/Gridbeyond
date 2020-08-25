using System;
using System.Collections.Generic;

namespace GridBeyond.Domain.Models
{
    public class ReportDataGroupModel
    {
        public DateTime Date { get; set; }
        public double Average { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public List<DataModel> Events { get; internal set; }
    }
}