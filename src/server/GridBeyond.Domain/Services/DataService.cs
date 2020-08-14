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
    public class DataService : IDataService
    {
        public event EventHandler<int> OnMalformedRecord;
        public event EventHandler<ValidRecordEventArgs> OnValidRecord;

        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<DataModel>> GetAllData()
        {
            throw new System.NotImplementedException();
        }

        public Task InsertMultiple(IEnumerable<InsertDataModel> models)
        {
            throw new System.NotImplementedException();
        }

        public Task InsertRecord(InsertDataModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ValidationResult> ValidData(List<string> data)
        {
            var result = new ValidationResult();

            foreach (var record in data.Select((value, i) => new { i, value }))
            {
                if (IsValid(record.value, out DateTime date, out double marketPrice))
                {
                    var validRecord = new InsertDataModel
                    {
                        Date = date,
                        MarketpriceEX1 = marketPrice
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

        private bool IsValid(string value, out DateTime date, out double marketPrice)
        {
            date = default;
            marketPrice = default;

            var split = value.Split(',');

            if (split.Length != 2)
                return false;
            if (!DateTime.TryParse(split[0], out date))
                return false;
            if (!double.TryParse(split[1], out marketPrice))
                return false;

            return true;
        }
    }
}