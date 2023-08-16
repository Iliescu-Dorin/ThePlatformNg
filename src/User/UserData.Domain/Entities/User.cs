using Core.SharedKernel.Constants;
using Core.SharedKernel.Data.Enums;
using Core.SharedKernel.Entities;
using System.Runtime;

namespace UserData.Domain.Entities;

public class User : Entity<Guid>
{
    public User(Guid id) : base(id) { }

    public required string EmailAddress { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required UserRole Role { get; set; }
    public required string Place { get; set; }
    public required string ImageUrl { get; set; } = Profile.DefaultImage;
}
