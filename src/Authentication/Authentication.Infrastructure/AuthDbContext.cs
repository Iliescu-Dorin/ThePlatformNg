
using Authentication.Domain;
using Authentication.Infrastructure.EntityTypeConfigurations;
using Core.Services.APIs.Interfaces;
using Core.SharedKernel.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure;
public class AuthDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    private readonly ICurrentUserService _currentUserService;


    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new RefreshTokenConfigurations());

        // Customize ASP.NET Identity models and override defaults
        // such as renaming ASP.NET Identity, changing key types etc.
        modelBuilder.ApplyConfiguration(new ApplicationUserConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityRoleClaimConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityUserClaimConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityUserLoginConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityRoleConfigurations());
        modelBuilder.ApplyConfiguration(new IdentityUserTokenConfigurations());
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
