using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;

namespace StockMarket.Data
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.Property(o => o.Id).ValueGeneratedNever();
                b.Property(o => o.Side).HasConversion<int>();
                b.Property(o => o.Price).HasColumnType("money");
                b.Property(o => o.Quantity).HasColumnType("money");
            });
        }
    }
}