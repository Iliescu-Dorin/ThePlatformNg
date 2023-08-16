using Core.SharedKernel.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.DTO;
public class CreateOrUpdateUserDTO
{
    public required string EmailAddress { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
    public required string Place { get; set; }
    public required string ImageUrl { get; set; }
}
