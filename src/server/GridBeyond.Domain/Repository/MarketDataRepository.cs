using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Repository
{
    public class MarketDataRepository : IMarketDataRepository
    {
        private static readonly List<MarketData> _data = new List<MarketData>
        {
            new MarketData{Date = DateTime.Now, Id = 1, MarketpriceEX1 = 1234.567d}
        };

        public async Task<IEnumerable<DataModel>> Get()
        {
            return await Task.Run(() =>
            {
                return _data.Select(x => new DataModel
                {
                    Date = x.Date,
                    MarketpriceEX1 = x.MarketpriceEX1
                });
            });
        }

        public async Task Insert(IEnumerable<InsertDataModel> models)
        {
            await Task.Run(() =>
            {
                foreach (var model in models)
                {
                    _data.Add(new MarketData
                    {
                        Date = model.Date,
                        Id = _data.Count + 1,
                        MarketpriceEX1 = model.MarketpriceEX1
                    });
                }
            });
        }

        public async Task Insert(InsertDataModel model)
        {
            await Task.Run(() =>
            {
                _data.Add(new MarketData
                {
                    Date = model.Date,
                    Id = _data.Count + 1,
                    MarketpriceEX1 = model.MarketpriceEX1
                });
            });
        }
    }
}