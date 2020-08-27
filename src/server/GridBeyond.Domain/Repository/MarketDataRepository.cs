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

        public async Task<IList<ReportDataGroupModel>> GetReportData(DateTime? start = null, DateTime? end = null)
        {
            var query = (from record in Get()
                         group record.MarketPriceEX1 by record.Date.Date
                        into g
                         select new
                         {
                             Date = g.Key,
                             Average = g.Average(),
                             Max = g.Max(),
                             Min = g.Min(),
                         });

            if (start.HasValue)
            {
                query = query.Where(x => x.Date >= start);
                if (end.HasValue)
                    query = query.Where(x => x.Date <= end);
            }

            return (await query.ToListAsync())
                .Select(x => new ReportDataGroupModel
                {
                    Date = x.Date,
                    Average = x.Average,
                    Max = x.Max,
                    Min = x.Min,
                    Events = Get(y => y.Date.Date == x.Date).ToList()
                }).ToList();
        }

        public async Task<IEnumerable<InsertDataModel>> Exists(IEnumerable<InsertDataModel> source)
        {
            var dates = source.Select(x => x.Date.Date).Distinct();

            var existingEntries = await Get(x => dates.Contains(x.Date.Date))
                .ToListAsync();

            return from existing in existingEntries
                   join model in source on new { existing.MarketPriceEX1, existing.Date }
                                   equals new { model.MarketPriceEX1, model.Date }
                   select model;
        }
    }
}