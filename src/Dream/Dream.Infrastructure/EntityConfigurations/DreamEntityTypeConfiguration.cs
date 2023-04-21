using DreamDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DreamInfrastructure.EntityConfigurations;
public class DreamEntityTypeConfiguration : IEntityTypeConfiguration<Dream>
{
    public void Configure(EntityTypeBuilder<Dream> builder)
    {
        builder.ToTable("Dreams");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).IsRequired();
        builder.Property(d => d.Title).IsRequired();
        builder.Property(d => d.Description).IsRequired();
        builder.Property(d => d.Date).IsRequired();
        builder.Property(d => d.UserId).IsRequired();
        //builder.HasOne(d => d.User).WithMany(u => u.Dreams).HasForeignKey(d => d.UserId);
        //builder.HasMany(d => d.Interpretations).WithOne(i => i.Dream).HasForeignKey(i => i.DreamId);
    }
}