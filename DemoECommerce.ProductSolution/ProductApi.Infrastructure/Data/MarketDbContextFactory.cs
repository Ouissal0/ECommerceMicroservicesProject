using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class MarketDbContextFactory : IDesignTimeDbContextFactory<MarketDbContext>
{
    public MarketDbContext CreateDbContext(string[] args)
    {
        // Configure the application settings file (appsettings.json)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Set the base directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Create DbContextOptionsBuilder
        var optionsBuilder = new DbContextOptionsBuilder<MarketDbContext>();

        // Get the connection string from appsettings.json
        var connectionString = configuration.GetConnectionString("eCommerceConnection");

        // Configure the DbContext to use SQL Server
        optionsBuilder.UseSqlServer(connectionString);

        // Return the DbContext instance
        return new MarketDbContext(optionsBuilder.Options);
    }
}
