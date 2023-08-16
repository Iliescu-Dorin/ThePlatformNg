using Core.Services.APIs.Interfaces;
using Core.SharedKernel.Entities;
using Microsoft.EntityFrameworkCore;
using UserData.Domain.Entities;
using UserData.Infrastructure.EntityConfiguration;

namespace UserData.Infrastructure;
public class UserDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;


    public DbSet<User> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public UserDbContext(DbContextOptions<UserDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(DreamContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }

        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
