using Core.SharedKernel.Entities;

namespace UserData.Domain.Entities;

public class User : Entity<Guid>
{
    public User(Guid id) : base(id)
    {
    }

    public new Guid Id { get; set; }
    public string? Username { get; set; }
    
    public string? Place { get; set; }
    public string? ImageUrl { get; set; }
}
