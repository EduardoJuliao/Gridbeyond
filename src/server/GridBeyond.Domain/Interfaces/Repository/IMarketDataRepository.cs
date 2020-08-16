using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Repository
{
    public interface IMarketDataRepository
    {
        Task<IEnumerable<DataModel>> Get();
        Task<IEnumerable<DataModel>> Get(Expression<Func<MarketData, bool>> expression);
        Task Insert(IEnumerable<InsertDataModel> models);
        Task Insert(InsertDataModel models);
    }
}