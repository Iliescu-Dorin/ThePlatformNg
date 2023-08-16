using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts.Config;
public class JwtTokenConfig
{
    public string Issuer { get; set; } = "thisismeyouknow";
    public string Audience { get; set; } = "thisismeyouknow";
    public int ExpiryInMinutes { get; set; } = 10;
    public string key { get; set; } = "thiskeyisverylargetobreak";
}
