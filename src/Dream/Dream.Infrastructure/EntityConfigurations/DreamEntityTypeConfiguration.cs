using DreamDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using System.Xml;

namespace DreamInfrastructure.EntityConfigurations;
public class DreamEntityTypeConfiguration : IEntityTypeConfiguration<Dream> {
  public void Configure(EntityTypeBuilder<Dream> builder) {
    builder.ToTable("Dream");
    builder.HasKey(d => d.Id);

    builder.Property(d => d.UserId).IsRequired();
    builder.Property(d => d.Title).IsRequired().HasMaxLength(50);
    builder.Property(d => d.Description).IsRequired().HasMaxLength(1000);
    builder.Property(d => d.Date).IsRequired();
    builder.Property(d => d.Id).IsRequired();
    builder.Property(d => d.Symbols)
        .IsRequired()
        .HasMaxLength(500)
        .HasConversion(
            v => JsonSerializer.Serialize(
                v, new JsonSerializerOptions { PropertyNamingPolicy =
                                                   JsonNamingPolicy.CamelCase,
                                               WriteIndented = true }),
            v => JsonSerializer.Deserialize<List<string>>(
                v,
                new JsonSerializerOptions { PropertyNamingPolicy =
                                                JsonNamingPolicy.CamelCase }),
            new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) =>
                                        HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));
    // builder.HasOne(d => d.User).WithMany(u => u.Dreams).HasForeignKey(d =>
    // d.UserId); builder.HasMany(d => d.Interpretations).WithOne(i =>
    // i.Dream).HasForeignKey(i => i.DreamId);
  }
}
