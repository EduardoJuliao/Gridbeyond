using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Repository
{
    public interface IMarketDataRepository
    {
        IQueryable<DataModel> Get();
        IQueryable<DataModel> Get(Expression<Func<MarketData, bool>> expression);
        IQueryable<ReportDataGroupModel> GetReportData(DateTime? start = null, DateTime? end = null);
        int Count();
        bool Exists(Expression<Func<MarketData, bool>> expression);
        Task Insert(IEnumerable<InsertDataModel> models);
        Task Insert(InsertDataModel models);
    }
}