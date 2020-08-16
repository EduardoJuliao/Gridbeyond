using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.EventArgs;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GridBeyond.Domain.Services
{
    public class MarketDataService : IMarketDataService
    {
        private readonly IMarketDataRepository _repository;
        private event EventHandler<int> OnMalformedRecord;
        private event EventHandler<ValidRecordEventArgs> OnValidRecord;
        private event EventHandler<IEnumerable<InsertDataModel>> OnInsertRecord;

        public MarketDataService(IMarketDataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<DataModel>> GetAllData()
        {
            return await _repository.Get().ToListAsync();
        }

        public async Task InsertMultiple(IEnumerable<InsertDataModel> models)
        {
            var newModels = models.Where(x =>
                !_repository.Exists(y => x.Date == y.Date && x.MarketPriceEX1 == y.MarketPriceEX1))
                .ToArray();
            
            if (newModels.Any())
            {
                await _repository.Insert(newModels);
                OnInsertRecord?.Invoke(this, newModels);
            }
        }

        public async Task InsertRecord(InsertDataModel model)
        {
            if (!_repository.Exists(y => model.Date == y.Date && model.MarketPriceEX1 == y.MarketPriceEX1))
            {
                await _repository.Insert(model);
                OnInsertRecord?.Invoke(this, new List<InsertDataModel> {model});
            }
        }

        public Task<ValidationResult> ValidData(IEnumerable<string> data)
        {
            var result = new ValidationResult();

            foreach (var record in data.Select((value, i) => new {i, value}))
            {
                if (IsValid(record.value, out var date, out var marketPrice))
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

            return Task.FromResult(result);
        }

        public async Task<ReportData> GetReportDataHistory()
        {
            var c = (from record in _repository.Get()
                    group record.MarketPriceEX1 by record.Date
                    into g
                    select new
                    {
                        Date = g.Key,
                        Values = g.ToList(),
                        Avarage = g.Average(),
                        Max = g.Max(),
                        Min = g.Min(),
                        AvarageTotal = g.Average(),
                        MaxTotal = g.Max(),
                        MinTotal = g.Min()
                    })
                .Select(x => new ReportData
                {
                    AverageValue = x.AvarageTotal,
                    HighestValue = x.MaxTotal,
                    LowestValue = x.MinTotal,
                })
                .ToListAsync();


            throw new NotImplementedException();
        }

        public Task<ReportData> GetReportDataPeriod(DateTime start, DateTime? end)
        {
            throw new NotImplementedException();
        }

        public void AddOnMalformedRecordEvent(EventHandler<int> callback) => OnMalformedRecord += callback;

        public void AddOnValidRecord(EventHandler<ValidRecordEventArgs> callback) => OnValidRecord += callback;

        public void AddOnInsertedRecord(EventHandler<IEnumerable<InsertDataModel>> callback) =>
            OnInsertRecord += callback;

        private static bool IsValid(string value, out DateTime date, out double marketPrice)
        {
            date = default;
            marketPrice = default;

            var split = value.Split(',');

            if (split.Length != 2)
                return false;

            return DateTime.TryParse(split[0], out date) && double.TryParse(split[1], out marketPrice);
        }
    }
}