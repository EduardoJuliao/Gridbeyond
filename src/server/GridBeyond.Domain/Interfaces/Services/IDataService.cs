using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Services
{
    public interface IDataService
    {
        Task<IEnumerable<DataModel>> GetAllData();
        Task InsertRecord(InsertDataModel model);
        Task InsertMultiple(IEnumerable<InsertDataModel> models);
        Task<ValidationResult> ValidData(List<string> data);

        event EventHandler<int> OnMalformedRecord;
        event EventHandler<ValidRecordEventArgs> OnValidRecord;
    }
}