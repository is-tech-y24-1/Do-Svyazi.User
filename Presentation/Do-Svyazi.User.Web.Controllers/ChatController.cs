using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator) => _mediator = mediator;

    [HttpGet(nameof(GetChats))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MessengerChatDto>>> GetChats()
    {
        var response = await _mediator.Send(new GetChats.Query());
        return Ok(response.chats);
    }

    [HttpGet(nameof(GetChatById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerChatDto>> GetChatById(Guid chatId)
    {
        var response = await _mediator.Send(new GetChatById.Query(chatId));
        return Ok(response.chat);
    }

    [HttpGet(nameof(GetUserIdsByChatId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<Guid>>> GetUserIdsByChatId(Guid chatId)
    {
        var response = await _mediator.Send(new GetUserIdsByChatId.Query(chatId));
        return Ok(response);
    }

    [HttpGet(nameof(GetUsersByChatId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<ChatUser>>> GetUsersByChatId(Guid chatId)
    {
        var response = await _mediator.Send(new GetUsersByChatId.Query(chatId));
        return Ok(response);
    }

    [HttpPost(nameof(AddChannel))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddChannel(Guid userId, string name, string description)
    {
        var response = await _mediator.Send(new AddChannel.Command(userId, name, description));
        return Ok(response);
    }

    [HttpPost(nameof(AddGroupChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddGroupChat(Guid userId, string name, string description)
    {
        var response = await _mediator.Send(new AddGroupChat.Command(userId, name, description));
        return Ok(response);
    }

    [HttpPost(nameof(AddPersonalChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddPersonalChat(Guid firstUserId, Guid secondUserId, string name, string description)
    {
        var response = await _mediator.Send(new AddPersonalChat.Command(firstUserId, secondUserId, name, description));
        return Ok(response);
    }

    [HttpPost(nameof(AddSavedMessages))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddSavedMessages(Guid userId, string name, string description)
    {
        var response = await _mediator.Send(new AddSavedMessages.Command(userId, name, description));
        return Ok(response);
    }

    [HttpPost(nameof(AddUserToChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddUserToChat(Guid userId, Guid chatId)
    {
        await _mediator.Send(new AddUserToChat.Command(userId, chatId));
        return Ok();
    }

    [HttpDelete(nameof(DeleteUserToChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteUserToChat(Guid userId, Guid chatId)
    {
        await _mediator.Send(new DeleteUserToChat.Command(userId, chatId));
        return Ok();
    }
}