using System;
using System.Collections.Generic;

namespace GridBeyond.Domain.Models
{
    public class ValidationResult
    {
        public readonly List<int> MalformedRecordLine = new List<int>();
        public readonly List<InsertDataModel> ValidRecord = new List<InsertDataModel>(); 
    }
}
