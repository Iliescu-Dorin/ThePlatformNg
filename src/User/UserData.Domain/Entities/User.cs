using Core.SharedKernel.Entities;

namespace UserData.Domain.Entities;

public class User 
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Place { get; set; }
    public string? ImageUrl { get; set; }
}
