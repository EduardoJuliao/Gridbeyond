using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Interfaces.Services
{
    public interface IProcessHistoryService
    {
        Task SaveProcess(int validRecords, int malformedRecords, int newRecords, int totalRecords);
        Task<List<ProcessHistoryModel>> GetProcessHistory();
    }
}