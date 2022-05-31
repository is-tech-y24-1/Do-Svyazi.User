using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Domain.Users;
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
    public async Task<ActionResult<IReadOnlyCollection<MessengerUserDto>>> GetUsers()
    {
        var response = await _mediator.Send(new GetUsers.Query());
        return Ok(response.users);
    }

    [HttpGet("GetUserById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetUser(Guid userId)
    {
        var response = await _mediator.Send(new GetUser.Command(userId));
        return Ok(response);
    }

    [HttpGet("GetAllChatsByUserId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetAllChatsByUserId(Guid userId)
    {
        var response = await _mediator.Send(new GetAllChatsByUserId.Command(userId));
        return Ok(response);
    }

    [HttpGet("GetAllChatsIdByUserId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetAllChatsIdByUserId(Guid userId)
    {
        var response = await _mediator.Send(new GetAllChatsIdByUserId.Command(userId));
        return Ok(response);
    }

    [HttpGet("GetUserRoleByChatId")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetUserRoleByChatId(Guid userId, Guid chatId)
    {
        var response = await _mediator.Send(new GetUserRoleByChatId.Command(userId, chatId));
        return Ok(response);
    }

    [HttpPost("ChangeNickName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetNickNameById(Guid userId, string nickName)
    {
        await _mediator.Send(new SetUserNickNameById.Command(userId, nickName));
        return Ok();
    }

    [HttpPost("DeleteUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        await _mediator.Send(new DeleteUser.Command(userId));
        return Ok();
    }

    [HttpPost("AddChatToUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddChatToUser(Guid userId, Guid chatId)
    {
        await _mediator.Send(new AddChatToUser.Command(userId, chatId));
        return Ok();
    }

    [HttpPost("AddUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddUser(string name, string nickName, string description)
    {
        Guid response = await _mediator.Send(new AddUser.Command(name, nickName, description));
        return Ok(response);
    }

    [HttpPost("ChangeDescription")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeDescription(Guid userId,  string description)
    {
        await _mediator.Send(new ChangeUserDescriptionById.Command(userId, description));
        return Ok();
    }

    [HttpPost("ChangeName")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeName(Guid userId,  string name)
    {
        await _mediator.Send(new ChangeUserNameById.Command(userId, name));
        return Ok();
    }
}