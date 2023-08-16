using DreamData.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DreamData.API;

public class DreamDbContextFactory : IDesignTimeDbContextFactory<DreamDbContext>
{
    public DreamDbContext CreateDbContext(string[] args)
    {
        IConfiguration localConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

        var configForDb = localConfig.GetSection("Database");

        var dbConnectionString = configForDb.GetSection("DreamApi");

        var identityDbContextOptionsBuilder = new DbContextOptionsBuilder<DreamDbContext>()
            .UseSqlServer(dbConnectionString.Value, opt => opt.MigrationsAssembly("DreamWebApi"));

        return new DreamDbContext(identityDbContextOptionsBuilder.Options);
    }
}
