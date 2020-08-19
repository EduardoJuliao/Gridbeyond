using System;

namespace GridBeyond.Domain.Models
{
    public class ProcessHistoryModel
    {
        public DateTime ProcessDate { get; set; }
        public int TotalRecords { get; set; }
        public int ValidRecords { get; set; }
        public int MalformedRecords { get; set; }
        public int NewRecords { get; set; }
    }
}