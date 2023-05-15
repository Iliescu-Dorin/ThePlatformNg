using DreamData.Application.Services.Interfaces;
using DreamData.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DreamData.API.Controllers;

[ApiController]
public class DreamsController : ControllerBase
{
    private readonly IMediator _mediator;
    public readonly IDreamDataService _dreamDataService;

    public DreamsController(IMediator mediator, IDreamDataService dreamDataService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _dreamDataService = dreamDataService ?? throw new ArgumentNullException(nameof(dreamDataService));
    }

    [HttpGet]
    [MapToApiVersion("1")]
    [Route("api/v{version:apiVersion}/dreams")]
    public async Task<List<Dream>> GetAllDreams()
    {
        var res = await _dreamDataService.GetAllDreams();
        return res;
    }
}
