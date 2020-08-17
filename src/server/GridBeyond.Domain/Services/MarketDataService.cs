using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ValidationResult> ValidData(IEnumerable<string> data)
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

            return await Task.FromResult(result);
        }

        public async Task<ReportData> GetReportDataHistory()
        {
            var query = await (from record in _repository.Get()
                group record.MarketPriceEX1 by record.Date
                into g
                select new ReportDataGroupModel
                {
                    Date = g.Key,
                    Average = g.Average(),
                    Max = g.Max(),
                    Min = g.Min(),
                }).ToListAsync();

            return new ReportData
            {
                AverageValue = query.Average(x => x.Average),
                HighestValue = query.Max(x => x.Max),
                LowestValue = query.Min(x => x.Min),
                LowestValueDate = query.Single(x => x.Min == query.Min(y => y.Min)).Date,
                HighestValueDate = query.Single(x => x.Max == query.Max(y => y.Max)).Date
            };
        }

        public async Task<ReportData> GetReportDataPeriod(DateTime start, DateTime? end)
        {
            if (!end.HasValue)
                end = DateTime.Now;
            
            var query = await (from record in _repository.Get()
                where record.Date >= start && record.Date <= end
                group record.MarketPriceEX1 by record.Date
                into g
                select new ReportDataGroupModel
                {
                    Date = g.Key,
                    Average = g.Average(),
                    Max = g.Max(),
                    Min = g.Min(),
                }).ToListAsync();

            return new ReportData
            {
                AverageValue = query.Average(x => x.Average),
                HighestValue = query.Max(x => x.Max),
                LowestValue = query.Min(x => x.Min),
                LowestValueDate = query.Single(x => x.Min == query.Min(y => y.Min)).Date,
                HighestValueDate = query.Single(x => x.Max == query.Max(y => y.Max)).Date
            };
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
            
            var formats = new string []{"dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy HH:mm","dd/MM/yyyy"};

            if (!DateTime.TryParseExact(split[0], formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out date))
                return false;

            if (!double.TryParse(split[1], out marketPrice))
                return false;

            return true;
        }
    }
}