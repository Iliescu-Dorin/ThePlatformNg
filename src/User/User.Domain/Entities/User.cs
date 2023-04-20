using Core.SharedKernel.Entities;

namespace User.Domain.Entities;

public class User : Entity
{
    public int Id { get; set; }
    public string? Username { get; set; }
    p
    public string? Place { get; set; }
    public string? ImageUrl { get; set; }
}
