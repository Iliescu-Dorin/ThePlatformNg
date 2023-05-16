using MediatR;
using Microsoft.AspNetCore.Mvc;
using UserData.API.Services.Interfaces;
using UserData.Domain.Entities;

namespace UserData.API.Controllers;

[ApiController]
public class UserController : Controller
{
    private readonly IMediator _mediator;
    public readonly IUserService _userService;

    public UserController(IMediator mediator, IUserService userService)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

    [HttpGet]
    [MapToApiVersion("1")]
    [Route("api/v{version:apiVersion}/users")]
    public async Task<List<User>> GetAllUsers()
    {
        var res = await _userService.GetAllUsers();
        return res;
    }
}
