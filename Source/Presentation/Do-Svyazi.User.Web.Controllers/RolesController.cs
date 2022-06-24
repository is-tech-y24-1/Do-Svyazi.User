using Do_Svyazi.User.Application.CQRS.Roles.Commands;
using Do_Svyazi.User.Application.CQRS.Roles.Queries;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet(nameof(GetRoleByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RoleDto>> GetRoleByUserId(
        [FromQuery] GetRoleByUserIdQuery getRoleByUserIdQuery, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(getRoleByUserIdQuery, cancellationToken);
        return Ok(response);
    }

    [HttpPost(nameof(CreateRoleForChat))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateRoleForChat(
        CreateRoleForChatCommand createRoleForChatCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(createRoleForChatCommand, cancellationToken);
        return Ok();
    }

    [HttpPost(nameof(ChangeRoleForUserById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeRoleForUserById(
        ChangeRoleForUserByIdCommand changeRoleForUserByIdCommand, CancellationToken cancellationToken)
    {
        await _mediator.Send(changeRoleForUserByIdCommand, cancellationToken);
        return Ok();
    }
}