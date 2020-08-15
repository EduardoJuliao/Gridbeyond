using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.EventArgs;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Services
{
    public class MarketDataService : IMarketDataService
    {

        readonly IMarketDataRepository _repository;
        event EventHandler<int> OnMalformedRecord;
        event EventHandler<ValidRecordEventArgs> OnValidRecord;
        event EventHandler<IEnumerable<InsertDataModel>> OnInsertRecord;

        public MarketDataService(IMarketDataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IEnumerable<DataModel>> GetAllData()
        {
            return await _repository.Get();
        }

        public async Task InsertMultiple(IEnumerable<InsertDataModel> models)
        {
                await _repository.Insert(models);
                //_events.OnInsertRecord.Invoke(this, models);
        }

        public async Task InsertRecord(InsertDataModel model)
        {
            await _repository.Insert(model);
            //OnInsertRecord?.Invoke(this, new List<InsertDataModel> {model});
        }

        public Task<ValidationResult> ValidData(List<string> data)
        {
            var result = new ValidationResult();

            foreach (var record in data.Select((value, i) => new {i, value}))
            {
                if (IsValid(record.value, out var date, out var marketPrice))
                {
                    var validRecord = new InsertDataModel
                    {
                        Date = date,
                        MarketpriceEX1 = marketPrice
                    };
                    // OnValidRecord?.Invoke(this, new ValidRecordEventArgs
                    // {
                    //     InsertData = validRecord,
                    //     Row = record.i
                    // });
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

        public void AddOnMalformedRecordEvent(EventHandler<int> callback)
        {
            OnMalformedRecord += callback;
        }

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