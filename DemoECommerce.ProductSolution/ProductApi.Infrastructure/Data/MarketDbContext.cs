using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

public class MarketDbContext : DbContext
{
   
    public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options) { }

    public DbSet<Market> Markets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Market>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

}
