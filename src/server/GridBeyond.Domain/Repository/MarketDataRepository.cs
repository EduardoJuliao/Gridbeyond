using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Models;

namespace GridBeyond.Domain.Repository
{
    public class MarketDataRepository : IMarketDataRepository
    {
        private readonly MarketContext _context;

        public MarketDataRepository(MarketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public IQueryable<DataModel> Get()
        {
            return _context.MarketDatas.Select(x => new DataModel
            {
                Date = x.Date,
                MarketPriceEX1 = x.MarketPriceEX1
            });
        }

        public IQueryable<DataModel> Get(Expression<Func<MarketData, bool>> expression)
        {
            return _context.MarketDatas
                .Where(expression)
                .Select(x => new DataModel
                {
                    Date = x.Date,
                    MarketPriceEX1 = x.MarketPriceEX1
                });
        }

        public int Count()
        {
            return _context.MarketDatas.Count();
        }

        public bool Exists(Expression<Func<MarketData, bool>> expression)
        {
            return _context.MarketDatas.Any(expression);
        }

        public async Task Insert(IEnumerable<InsertDataModel> models)
        {
            await _context.MarketDatas.AddRangeAsync(models.Select(x => new MarketData
            {
                Date = x.Date,
                MarketPriceEX1 = x.MarketPriceEX1
            }));

            await _context.SaveChangesAsync();
        }

        public async Task Insert(InsertDataModel model)
        {
            await _context.MarketDatas.AddAsync(new MarketData
            {
                Date = model.Date,
                MarketPriceEX1 = model.MarketPriceEX1
            });
            await _context.SaveChangesAsync();
        }
    }
}