using DreamDomain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DreamWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DreamsController : ControllerBase
{

    private readonly ILogger<DreamsController> _logger;

    public DreamsController(ILogger<DreamsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Dream> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Dream { })
        .ToArray();
    }
}
