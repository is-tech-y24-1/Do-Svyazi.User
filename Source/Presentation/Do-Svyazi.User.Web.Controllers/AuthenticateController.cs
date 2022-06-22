using System.IdentityModel.Tokens.Jwt;
using Do_Svyazi.User.Application.CQRS.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Do_Svyazi.User.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] Login model, CancellationToken cancellationToken)
        {
            JwtSecurityToken token = await _mediator.Send(model, cancellationToken);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] Register model, CancellationToken cancellationToken)
        {
            await _mediator.Send(model, cancellationToken);
            return Ok();
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterAdmin model, CancellationToken cancellationToken)
        {
            await _mediator.Send(model, cancellationToken);
            return Ok();
        }
    }
}