using Microsoft.EntityFrameworkCore;
using UserData.Domain.Entities;
using UserData.Infrastructure.EntityConfiguration;

namespace UserData.Infrastructure;
public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(DreamContext).Assembly);
    }
}
