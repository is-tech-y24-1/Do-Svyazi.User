using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator) => _mediator = mediator;

    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MessengerUserDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUsersQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerUser>> GetUser(
        [FromQuery] GetUserQuery getUserQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getUserQuery, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetAllChatsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MessengerChatDto>>> GetAllChatsByUserId(
        [FromQuery] GetAllChatsByUserIdQuery getAllChatsByUserIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getAllChatsByUserIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetAllChatsIdsByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Guid>>> GetAllChatsIdsByUserId(
        [FromQuery] GetAllChatsIdsByUserIdQuery getAllChatsIdsByUserIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getAllChatsIdsByUserIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(SetNickNameById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> SetNickNameById(
        SetUserNickNameByIdCommand setUserNickNameByIdCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(setUserNickNameByIdCommand, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(DeleteUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUser(DeleteUserCommand deleteUserCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(deleteUserCommand, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(AddUser))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> AddUser(AddUserCommand addUserCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addUserCommand, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(ChangeDescription))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeDescription(
        ChangeUserDescriptionByIdCommand changeUserDescriptionByIdCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(changeUserDescriptionByIdCommand, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(ChangeName))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeName(ChangeUserNameByIdCommand changeUserNameByIdCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(changeUserNameByIdCommand, cancellationToken);
        return Ok();
    }
}