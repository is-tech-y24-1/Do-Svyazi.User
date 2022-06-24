using Do_Svyazi.User.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Do_Svyazi.User.Web.Controllers.Tools;

public class ExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public async void OnException(ExceptionContext context)
    {
        Exception exception = context.Exception;

        if (exception.GetType() == typeof(Do_Svyazi_User_BusinessLogicException))
        {
            context.HttpContext.Response.StatusCode = 400;
        }
        else if (exception.GetType() == typeof(UnauthorizedAccessException))
        {
            context.HttpContext.Response.StatusCode = 401;
        }
        else if (exception.GetType() == typeof(Do_Svyazi_User_NotFoundException))
        {
            context.HttpContext.Response.StatusCode = 404;
        }
        else
        {
            context.HttpContext.Response.StatusCode = 500;
        }

        await context.HttpContext.Response.WriteAsJsonAsync(
            $"An exception occurred in the {context.ActionDescriptor.DisplayName} method:" +
            $"\n {exception.StackTrace} \n {exception.Message}");

        context.ExceptionHandled = true;
    }
}