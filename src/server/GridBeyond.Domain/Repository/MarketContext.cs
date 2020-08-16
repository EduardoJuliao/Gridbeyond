using GridBeyond.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GridBeyond.Domain.Repository
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options)
            : base(options)
        {
        }

        public DbSet<MarketData> MarketDatas { get; set; } 
    }
}