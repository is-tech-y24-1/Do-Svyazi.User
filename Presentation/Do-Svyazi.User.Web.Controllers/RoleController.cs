using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Application.CQRS.Roles;
using Do_Svyazi.User.Application.CQRS.Roles.Commands;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator) => _mediator = mediator;

    [HttpPost(nameof(CreateRoleForChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateRoleForChat(RoleDto role, Guid chatId)
    {
        await _mediator.Send(new CreateRoleForChat.Command(role, chatId));
        return Ok();
    }
}