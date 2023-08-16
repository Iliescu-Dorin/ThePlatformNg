using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.DTO;
public class AuthTokenDTO
{
    public required string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
}
