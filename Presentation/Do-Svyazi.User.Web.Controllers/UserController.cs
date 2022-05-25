using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Dtos.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<MessengerUserDto>>> GetUsers()
    {
        var response = await _mediator.Send(new GetUsers.Query());
        return Ok(response.Users);
    }

    [HttpPost("ChangeNickName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetNickNameById(Guid userId, string nickName)
    {
        await _mediator.Send(new SetUserNickNameById.Command(userId, nickName));
        return Ok();
    }

    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddUser(string name, string nickName, string description)
    {
        var response = await _mediator.Send(new AddUser.Command(name, nickName, description));
        return Ok(response);
    }
}