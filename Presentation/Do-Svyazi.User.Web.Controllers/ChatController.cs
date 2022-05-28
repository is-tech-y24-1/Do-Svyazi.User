using Do_Svyazi.User.Application.CQRS.Chats.Queries;
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

    [HttpGet("GetAll")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<MessengerChatDto>>> GetChats()
    {
        var response = await _mediator.Send(new GetChats.Query());
        return Ok(response.chats);
    }

    [HttpGet("GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessengerChatDto>> GetChatById(Guid chatId)
    {
        var response = await _mediator.Send(new GetChatById.Query(chatId));
        return Ok(response.chat);
    }
}