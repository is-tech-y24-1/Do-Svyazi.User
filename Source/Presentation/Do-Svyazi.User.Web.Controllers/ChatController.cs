using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Web.Controllers.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator) => _mediator = mediator;

    [HttpGet(nameof(GetChats))]
    public async Task<ActionResult<IReadOnlyCollection<MessengerChatDto>>> GetChats(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetChatsQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetChatById))]
    public async Task<ActionResult<MessengerChatDto>> GetChatById(
        [FromQuery] GetChatByIdQuery getChatByIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getChatByIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetUserIdsByChatId))]
    public async Task<ActionResult<IReadOnlyCollection<Guid>>> GetUserIdsByChatId(
        [FromQuery] GetUserIdsByChatIdQuery getUserIdsByChatIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getUserIdsByChatIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpGet(nameof(GetUsersByChatId))]
    public async Task<ActionResult<IReadOnlyCollection<ChatUser>>> GetUsersByChatId(
        [FromQuery] GetUsersByChatIdQuery getUsersByChatIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getUsersByChatIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(AddChannel))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddChannel(
        AddChannelCommand addChannelCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addChannelCommand, cancellationToken);
        return Created(nameof(AddChannel), response);
    }

    [HttpPost(nameof(AddGroupChat))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddGroupChat(
        AddGroupChatCommand addGroupChatCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addGroupChatCommand, cancellationToken);
        return Created(nameof(AddGroupChat), response);
    }

    [HttpPost(nameof(AddPersonalChat))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddPersonalChat(
        AddPersonalChatCommand addPersonalChatCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addPersonalChatCommand, cancellationToken);
        return Created(nameof(AddPersonalChat), response);
    }

    [HttpPost(nameof(AddSavedMessages))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> AddSavedMessages(
        AddSavedMessagesCommand addSavedMessagesCommand, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(addSavedMessagesCommand, cancellationToken);
        return Created(nameof(AddSavedMessages), response);
    }

    [HttpPost(nameof(AddUserToChat))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> AddUserToChat(
        AddUserToChatCommand addUserToChatCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(addUserToChatCommand, cancellationToken);
        return NoContent();
    }

    [HttpDelete(nameof(DeleteUserFromChat))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteUserFromChat(
        DeleteUserFromChatCommand deleteUserFromChatCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(deleteUserFromChatCommand, cancellationToken);
        return NoContent();
    }
}