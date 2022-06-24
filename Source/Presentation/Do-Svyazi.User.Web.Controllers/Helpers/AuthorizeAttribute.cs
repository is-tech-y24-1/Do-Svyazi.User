using Do_Svyazi.User.Domain.Authenticate;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Do_Svyazi.User.Web.Controllers.Helpers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.Items["User"] as MessageIdentityUser;

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }
    }
}