using System.IdentityModel.Tokens.Jwt;
using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Authenticate.Queries;
using Do_Svyazi.User.Domain.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticateController(IMediator mediator) => _mediator = mediator;

    [HttpGet(nameof(GetAll))]
    [Authorize]
    public async Task<IReadOnlyCollection<MessageIdentityUser>> GetAll(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUsersRequest(), cancellationToken);
        return response;
    }

    [HttpPost(nameof(Login))]
    public async Task<ActionResult> Login([FromBody] LoginRequest model, CancellationToken cancellationToken)
    {
        JwtSecurityToken token = await _mediator.Send(model, cancellationToken);
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo,
        });
    }

    [HttpPost(nameof(Register))]
    public async Task<ActionResult> Register([FromBody] RegisterCommand model, CancellationToken cancellationToken)
    {
        await _mediator.Send(model, cancellationToken);
        return Ok();
    }

    [HttpGet(nameof(AuthenticateByJwt))]
    public async Task<ActionResult<Guid>> AuthenticateByJwt(
        [FromHeader] string jwtToken, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AuthenticateByJwtRequest(jwtToken), cancellationToken);
        return Ok(result);
    }

    [HttpPost(nameof(RegisterAdmin))]
    [Authorize]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterAdminCommand model, CancellationToken cancellationToken)
    {
        await _mediator.Send(model, cancellationToken);
        return Ok();
    }
}