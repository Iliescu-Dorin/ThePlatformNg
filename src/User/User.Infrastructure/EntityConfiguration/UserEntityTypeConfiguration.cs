using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserData.Domain.Entities;

namespace UserData.Infrastructure.EntityConfiguration;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users"); // Set the table name

        // Configure primary key
        builder.HasKey(u => u.Id);

        // Configure properties
        builder.Property(u => u.Id)
            .HasColumnName("UserId")
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Username)
            .HasMaxLength(50);

        builder.Property(u => u.Place)
            .HasMaxLength(100);

        builder.Property(u => u.ImageUrl)
            .HasMaxLength(200);

        // Optionally, you can configure any additional properties or constraints here

        // Example: builder.Property(u => u.SomeProperty)
        //                .IsRequired();

        // No relationships provided in the User entity, but if there were, you would configure them here.

        // Optionally, you can configure data seeding
        // Example: builder.HasData(new User { Id = 1, Username = "john_doe", Place = "New York", ImageUrl = "profile.jpg" });
    }
}
