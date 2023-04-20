﻿using DreamDomain.Entities;
using DreamInfrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
namespace DreamInfrastructure;
public partial class DreamContext : DbContext
{
    public DbSet<Dream> Dreams { get; set; }
    public DbSet<Interpretation> Interpretations { get; set; }
    public DreamContext(DbContextOptions<DreamContext> options) : base(options)
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
