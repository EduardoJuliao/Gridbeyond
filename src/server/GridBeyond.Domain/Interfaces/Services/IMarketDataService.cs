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
        Task InsertRecord(InsertDataModel model);
        Task<InsertDataModel[]> InsertMultiple(IEnumerable<InsertDataModel> models);
        Task<ValidationResult> ValidData(IEnumerable<string> data);
        Task<ReportData> GetReportDataHistory();
        Task<ReportData> GetReportDataPeriod(DateTime start, DateTime? end);

        void AddOnMalformedRecordEvent(EventHandler<int> callback);
        void AddOnValidRecord(EventHandler<ValidRecordEventArgs> callback);
        void AddOnInsertedRecord(EventHandler<IEnumerable<InsertDataModel>> callback);
    }
}