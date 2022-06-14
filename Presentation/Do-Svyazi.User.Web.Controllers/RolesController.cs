using Do_Svyazi.User.Application.CQRS.Roles;
using Do_Svyazi.User.Application.CQRS.Roles.Commands;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpPost(nameof(CreateRoleForChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateRoleForChat(RoleDto role, Guid chatId)
    {
        await _mediator.Send(new CreateRoleForChat.Command(role, chatId));
        return Ok();
    }

    [HttpPost(nameof(ChangeRoleForUserById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeRoleForUserById(Guid userId, Guid chatId, RoleDto role)
    {
        await _mediator.Send(new ChangeRoleForUserById.Command(userId, chatId, role));
        return Ok();
    }

    [HttpGet(nameof(GetRoleByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Role>> GetRoleByUserId(Guid userId, Guid chatId)
    {
        Role response = await _mediator.Send(new GetRoleByUserId.Command(userId, chatId));
        return Ok(response);
    }
}