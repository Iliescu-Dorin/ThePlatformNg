using DreamInfrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DreamWebApi;

public class DreamDbContextFactory : IDesignTimeDbContextFactory<DreamContext> {
  public DreamContext CreateDbContext(string[] args) {
    IConfiguration localConfig =
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .Build();

    var configForDb = localConfig.GetSection("Database");

    var dbConnectionString = configForDb.GetSection("DreamApi");

    var identityDbContextOptionsBuilder =
        new DbContextOptionsBuilder<DreamContext>().UseSqlServer(
            dbConnectionString.Value,
            opt => opt.MigrationsAssembly("DreamWebApi"));

    return new DreamContext(identityDbContextOptionsBuilder.Options);
  }
}
