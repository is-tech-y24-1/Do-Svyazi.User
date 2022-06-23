using System.IdentityModel.Tokens.Jwt;
using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Authenticate.Queries;
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

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] Login model, CancellationToken cancellationToken)
    {
        JwtSecurityToken token = await _mediator.Send(model, cancellationToken);
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo,
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] Register model, CancellationToken cancellationToken)
    {
        await _mediator.Send(model, cancellationToken);
        return Ok();
    }

    [HttpGet("AuthenticateUserByJwt")]
    public async Task<ActionResult<Guid>> AuthenticateByJwt(
        [FromHeader] string jwtToken, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AuthenticateByJwt(jwtToken), cancellationToken);
        return Ok(result);
    }

    [HttpPost("register-admin")]
    [Authorize]
    public async Task<ActionResult> RegisterAdmin([FromBody] RegisterAdmin model, CancellationToken cancellationToken)
    {
        await _mediator.Send(model, cancellationToken);
        return Ok();
    }
}