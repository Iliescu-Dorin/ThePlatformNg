using Core.SharedKernel.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.DTO;
public class UserDTO
{
    public required string EmailAddress { get; set; }
    public string? Username { get; set; }
    public UserRole Role { get; set; }
    public string? Place { get; set; }
    public string? ImageUrl { get; set; }
}
