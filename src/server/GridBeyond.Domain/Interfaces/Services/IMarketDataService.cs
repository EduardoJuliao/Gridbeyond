using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Services
{
    public interface IMarketDataService
    {
        Task<IEnumerable<DataModel>> GetAllData();
        Task<IEnumerable<DataModel>> GetLatest(int recordsCount);
        Task InsertRecord(InsertDataModel model);
        Task<IEnumerable<InsertDataModel>> InsertMultiple(IEnumerable<InsertDataModel> models);
        Task<ValidationResult> ValidData(IEnumerable<string> data);
        Task<ReportData> GetReport(DateTime? start = null, DateTime? end = null);

        void AddOnMalformedRecordEvent(EventHandler<int> callback);
        void AddOnValidRecord(EventHandler<ValidRecordEventArgs> callback);
        void AddOnInsertedRecord(EventHandler<IEnumerable<InsertDataModel>> callback);
    }
}