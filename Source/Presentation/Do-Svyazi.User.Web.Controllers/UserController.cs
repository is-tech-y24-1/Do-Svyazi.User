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
    public async Task<ActionResult<IReadOnlyCollection<MessengerUserDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUsers(), cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetUser(
        [FromQuery] GetUser getUser, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getUser, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetAllChatsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MessengerChatDto>>> GetAllChatsByUserId(
        [FromQuery] GetAllChatsByUserId getAllChatsByUserId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getAllChatsByUserId, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetAllChatsIdsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Guid>>> GetAllChatsIdsByUserId(
        [FromQuery] GetAllChatsIdsByUserId getAllChatsIdsByUserId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getAllChatsIdsByUserId, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetUserRoleByChatId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Role>> GetUserRoleByChatId(
        [FromQuery] GetUserRoleByChatId getUserRoleByChatId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getUserRoleByChatId, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(SetNickNameById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetNickNameById(
        SetUserNickNameById setUserNickNameById, CancellationToken cancellationToken)
    {
        await _mediator.Send(setUserNickNameById, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(DeleteUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUser(DeleteUser deleteUser, CancellationToken cancellationToken)
    {
        await _mediator.Send(deleteUser, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(AddUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Guid>> AddUser(AddUser addUser, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addUser, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(ChangeDescription))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeDescription(
        ChangeUserDescriptionById changeUserDescriptionById, CancellationToken cancellationToken)
    {
        await _mediator.Send(changeUserDescriptionById, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(ChangeName))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeName(ChangeUserNameById changeUserNameById, CancellationToken cancellationToken)
    {
        await _mediator.Send(changeUserNameById, cancellationToken);
        return Ok();
    }
}