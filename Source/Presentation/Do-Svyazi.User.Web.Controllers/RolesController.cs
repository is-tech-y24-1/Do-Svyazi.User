using Do_Svyazi.User.Application.CQRS.Roles.Commands;
using Do_Svyazi.User.Application.CQRS.Roles.Queries;
using Do_Svyazi.User.Domain.Roles;
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
    public async Task<ActionResult> CreateRoleForChat(CreateRoleForChat createRoleForChat)
    {
        await _mediator.Send(createRoleForChat);
        return Ok();
    }

    [HttpPost(nameof(ChangeRoleForUserById))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> ChangeRoleForUserById(ChangeRoleForUserById changeRoleForUserById)
    {
        await _mediator.Send(changeRoleForUserById);
        return Ok();
    }

    [HttpGet(nameof(GetRoleByUserId))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Role>> GetRoleByUserId([FromQuery] GetRoleByUserId getRoleByUserId)
    {
        var response = await _mediator.Send(getRoleByUserId);
        return Ok(response);
    }
}