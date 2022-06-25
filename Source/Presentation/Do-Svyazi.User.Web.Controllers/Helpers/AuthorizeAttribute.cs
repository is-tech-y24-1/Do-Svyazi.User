// using Do_Svyazi.User.Domain.Authenticate;
// using Microsoft.AspNetCore.Mvc.Filters;
//
// namespace Do_Svyazi.User.Web.Controllers.Helpers;
//
// [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
// public class AuthorizeAttribute : Attribute, IAuthorizationFilter
// {
//     public AuthorizeAttribute() { }
//     public AuthorizeAttribute(bool isEnabled) => IsEnabled = isEnabled;
//
//     private bool IsEnabled { get; init; }
//
//     public void OnAuthorization(AuthorizationFilterContext context)
//     {
//         if (!IsEnabled) return;
//
//         if (context.HttpContext.Items["User"] is not MessageIdentityUser)
//             throw new UnauthorizedAccessException();
//     }
// }