using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GridBeyond.Domain.Services
{
    public class ProcessHistoryService : IProcessHistoryService
    {
        private readonly IProcessHistoryRepository _repository;

        public ProcessHistoryService(IProcessHistoryRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task SaveProcess(int validRecords, int malformedRecords, int newRecords, int totalRecords)
        {
            await _repository.SaveProcessHistory(new ProcessHistory
            {
                MalformedRecords = malformedRecords,
                NewRecords = newRecords,
                ProcessDate = DateTime.UtcNow,
                TotalRecords = totalRecords,
                ValidRecords = validRecords
            });
        }

        public async Task<List<ProcessHistoryModel>> GetProcessHistory()
        {
            return await _repository.Get().ToListAsync();
        }
    }
}