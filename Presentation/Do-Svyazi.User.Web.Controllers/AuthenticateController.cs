using System.IdentityModel.Tokens.Jwt;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Domain.Authenticate;
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
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            JwtSecurityToken token = await _mediator.Send(new Login.Command(model));
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            await _mediator.Send(new Register.Command(model));
            return Ok();
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            await _mediator.Send(new RegisterAdmin.Command(model));
            return Ok();
        }
    }
}