using Authentication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Infrastructure.EntityTypeConfigurations;
public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.ToTable(name: "Users");
        builder.Property("Email").HasColumnName("EmailAddress");
    }
}
