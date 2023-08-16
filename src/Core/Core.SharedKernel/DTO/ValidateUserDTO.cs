using Core.SharedKernel.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.DTO;
public class ValidateUserDTO
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
}
