using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Config;
public class JWTTokenConfig
{
    public string Issuer { get; set; } = "thisismeyouknow";
    public string Audience { get; set; } = "thisismeyouknow";
    public int ExpiryInMinutes { get; set; } = 10;
    public string key { get; set; } = "thiskeyisverylargetobreak";
}
