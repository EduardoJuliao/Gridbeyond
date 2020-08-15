using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Repository
{
    public interface IMarketDataRepository
    {
        Task<IEnumerable<DataModel>> Get();
        Task Insert(IEnumerable<InsertDataModel> models);
        Task Insert(InsertDataModel models);
    }
}