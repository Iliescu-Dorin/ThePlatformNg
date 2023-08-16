using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure.EntityTypeConfigurations;
public class IdentityUserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.ToTable(name: "UserRoles");
    }
}
