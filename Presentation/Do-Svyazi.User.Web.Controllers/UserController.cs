using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
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

    [HttpGet(nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetUser(Guid userId)
    {
        var response = await _mediator.Send(new GetUser.Query(userId));
        return Ok(response.messengerUser);
    }

    [HttpGet(nameof(GetAllChatsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MessengerChatDto>>> GetAllChatsByUserId(Guid userId)
    {
        var response = await _mediator.Send(new GetAllChatsByUserId.Query(userId));
        return Ok(response.users);
    }

    [HttpGet(nameof(GetAllChatsIdByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Guid>>> GetAllChatsIdByUserId(Guid userId)
    {
        var response = await _mediator.Send(new GetAllChatsIdsByUserId.Query(userId));
        return Ok(response.chatIds);
    }

    [HttpGet(nameof(GetUserRoleByChatId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Role>> GetUserRoleByChatId(Guid userId, Guid chatId)
    {
        var response = await _mediator.Send(new GetUserRoleByChatId.Query(userId, chatId));
        return Ok(response.role);
    }

    [HttpPost(nameof(SetNickNameById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetNickNameById(Guid userId, string nickName)
    {
        await _mediator.Send(new SetUserNickNameById.Command(userId, nickName));
        return Ok();
    }

    [HttpPost(nameof(DeleteUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUser(Guid userId)
    {
        await _mediator.Send(new DeleteUser.Command(userId));
        return Ok();
    }

    [HttpPost(nameof(AddUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> AddUser(string name, string nickName, string description)
    {
        Guid response = await _mediator.Send(new AddUser.Command(name, nickName, description));
        return Ok(response);
    }

    [HttpPost(nameof(ChangeDescription))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeDescription(Guid userId,  string description)
    {
        await _mediator.Send(new ChangeUserDescriptionById.Command(userId, description));
        return Ok();
    }

    [HttpPost(nameof(ChangeName))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeName(Guid userId,  string name)
    {
        await _mediator.Send(new ChangeUserNameById.Command(userId, name));
        return Ok();
    }
}