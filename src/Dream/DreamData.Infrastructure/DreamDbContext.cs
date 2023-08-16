using DreamData.Domain.Entities;
using DreamData.Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
namespace DreamData.Infrastructure;
public partial class DreamDbContext : DbContext
{
    public DbSet<Dream> Dreams { get; set; }
    public DbSet<Interpretation> Interpretations { get; set; }
    public DreamDbContext(DbContextOptions<DreamDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new DreamEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InterpretationEntityTypeConfiguration());
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(DreamContext).Assembly);
    }


}
