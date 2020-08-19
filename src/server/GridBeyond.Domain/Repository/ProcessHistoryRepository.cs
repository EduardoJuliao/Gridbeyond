using System;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Repository
{
    public class ProcessHistoryRepository : IProcessHistoryRepository
    {
        private readonly MarketContext _context;

        public ProcessHistoryRepository(MarketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task SaveProcessHistory(ProcessHistory ph)
        {
            await _context.AddAsync(ph);
            await _context.SaveChangesAsync();
        }

        public IQueryable<ProcessHistoryModel> Get()
        {
            return _context.ProcessHistories.Select(x => new ProcessHistoryModel
            {
                MalformedRecords = x.MalformedRecords,
                NewRecords = x.NewRecords,
                ProcessDate = x.ProcessDate,
                TotalRecords = x.TotalRecords,
                ValidRecords = x.ValidRecords
            });
        }
    }
}