using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;
using GridBeyond.Domain.Extensions;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using GridBeyond.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace GridBeyond.Domain.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IMarketDataRepository _repository;
        private readonly ICacheRepository _cacheRepository;

        private event EventHandler<int> OnMalformedRecord;
        private event EventHandler<ValidRecordEventArgs> OnValidRecord;
        private event EventHandler<IEnumerable<InsertDataModel>> OnInsertRecord;

        public void AddOnMalformedRecordEvent(EventHandler<int> callback) => OnMalformedRecord += callback;
        public void AddOnValidRecord(EventHandler<ValidRecordEventArgs> callback) => OnValidRecord += callback;
        public void AddOnInsertedRecord(EventHandler<IEnumerable<InsertDataModel>> callback) =>  OnInsertRecord += callback;

        public MarketDataService(IMarketDataRepository repository,
            ICacheRepository cacheRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<DataModel>> GetAllData()
        {
            return await _repository.Get().ToListAsync();
        }
        
        public async Task<IEnumerable<DataModel>> GetLatest(int recordsCount)
        {
            if (_cacheRepository.GetCache(CacheKeys.MarketKeys.LatestRecords, out IEnumerable<DataModel> latest))
            {
                var latestrecords = latest.OrderByDescending(x => x.Date).Take(recordsCount);
                return latest.OrderBy(x => x.Date);
            }

            latest = (await _repository.Get().OrderByDescending(x=> x.Date).Take(recordsCount).ToListAsync())
                .OrderBy(x => x.Date);

            _cacheRepository.SetOrUpdate(latest, CacheKeys.MarketKeys.LatestRecords);
            return latest;
        }

        public async Task<IEnumerable<InsertDataModel>> InsertMultiple(IEnumerable<InsertDataModel> models)
        {
            var existingModels = await _repository.Exists(models);

            var newModels = models.Except(existingModels);

            if (!newModels.Any()) return newModels;
            
            await _repository.Insert(newModels);
            OnInsertRecord?.Invoke(this, newModels);

            _cacheRepository.SetOrUpdate(newModels.OrderByDescending(x => x.Date).Take(50), CacheKeys.MarketKeys.LatestRecords);

            return newModels;
        }

        public async Task InsertRecord(InsertDataModel model)
        {
            if (!_repository.Exists(y => model.Date == y.Date && model.MarketPriceEX1 == y.MarketPriceEX1))
            {
                await _repository.Insert(model);
                OnInsertRecord?.Invoke(this, new List<InsertDataModel> {model});
            }
        }

        public async Task<ValidationResult> ValidData(IEnumerable<string> data)
        {
            var result = new ValidationResult();

            foreach (var record in data.RemoveDuplicates().Select((value, i) => new {i, value}))
            {
                if (record.value.IsValid(out var date, out var marketPrice))
                {
                    var validRecord = new InsertDataModel
                    {
                        Date = date,
                        MarketPriceEX1 = marketPrice
                    };
                    OnValidRecord?.Invoke(this, new ValidRecordEventArgs
                    {
                        InsertData = validRecord,
                        Row = record.i
                    });
                    result.ValidRecord.Add(validRecord);
                }
                else
                {
                    result.MalformedRecordLine.Add(record.i);
                    OnMalformedRecord?.Invoke(this, record.i);
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<ReportData> GetReport(DateTime? start = null, DateTime? end = null)
        {
            var data = await _repository.GetReportData(start, end);

            if (!data.Any())
                return new ReportData();

            return new ReportData
            {
                AverageValue = data.Average(x => x.Average),
                HighestValue = data.Max(x => x.Max),
                LowestValue = data.Min(x => x.Min),
                LowestValueDate = data.First(x => x.Min == data.Min(y => y.Min)).Date,
                HighestValueDate = data.First(x => x.Max == data.Max(y => y.Max)).Date,
                TotalRecords = _repository.Count(),
                PeakQuietPerDate = data.Select(x => new PeakQuiet
                {
                    Date = x.Date.Date,
                    PeakHours = x.Events.FindLatestPeakDuringDay(x.Date).ToArray(),
                    QuietHours = x.Events.FindLatestQuieterDuringDay(x.Date).ToArray()
                }).ToList()
            };
        }
    }
}