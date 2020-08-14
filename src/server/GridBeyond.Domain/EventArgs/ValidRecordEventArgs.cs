using System;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.EventArgs
{
    public class ValidRecordEventArgs
    {
        public int Row { get; set; }
        public InsertDataModel InsertData { get; set; }
    }
}
