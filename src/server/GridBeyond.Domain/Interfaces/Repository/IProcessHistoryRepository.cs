using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Repository
{
    public interface IProcessHistoryRepository
    {
        Task SaveProcessHistory(ProcessHistory ph);
        IQueryable<ProcessHistoryModel> Get();
    }
}