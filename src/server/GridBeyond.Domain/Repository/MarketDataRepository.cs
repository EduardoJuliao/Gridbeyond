using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GridBeyond.Domain.Repository
{
    public class MarketDataRepository : IMarketDataRepository
    {
        private readonly MarketContext _context;

        public MarketDataRepository(MarketContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        
        public async Task<IEnumerable<DataModel>> Get()
        {
            return await _context.MarketDatas.Select(x => new DataModel
            {
                Date = x.Date,
                MarketpriceEX1 = x.MarketpriceEX1
            }).ToListAsync();
        }

        public async Task<IEnumerable<DataModel>> Get(Expression<Func<MarketData, bool>> expression)
        {
            return await _context.MarketDatas
                .Where(expression)
                .Select(x => new DataModel
                {
                    Date = x.Date,
                    MarketpriceEX1 = x.MarketpriceEX1
                })
                .ToListAsync();
        }

        public async Task Insert(IEnumerable<InsertDataModel> models)
        {
            await _context.MarketDatas.AddRangeAsync(models.Select(x => new MarketData
            {
                Date = x.Date,
                MarketpriceEX1 = x.MarketpriceEX1
            }));

            await _context.SaveChangesAsync();
        }

        public async Task Insert(InsertDataModel model)
        {
            await _context.MarketDatas.AddAsync(new MarketData
            {
                Date = model.Date,
                MarketpriceEX1 = model.MarketpriceEX1
            });
            await _context.SaveChangesAsync();
        }
    }
}