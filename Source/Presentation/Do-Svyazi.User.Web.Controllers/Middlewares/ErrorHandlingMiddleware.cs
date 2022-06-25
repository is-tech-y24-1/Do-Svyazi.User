using Do_Svyazi.User.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Do_Svyazi.User.Web.Controllers.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Do_Svyazi_User_BusinessLogicException e)
        {
            context.Response.StatusCode = 400;
            await AttachErrorMessage(context, e);
        }
        catch (UnauthorizedAccessException e)
        {
            context.Response.StatusCode = 401;
            await AttachErrorMessage(context, e);
        }
        catch (Do_Svyazi_User_NotFoundException e)
        {
            context.Response.StatusCode = 404;
            await AttachErrorMessage(context, e);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await AttachErrorMessage(context, e);
        }
    }

    private async Task AttachErrorMessage(HttpContext context, Exception exception)
    {
        var message = new
        {
            message = exception.Message,
            endpoint = $"{context.GetEndpoint()}",
            stackTrace = exception.StackTrace,
        };

        await context.Response.WriteAsJsonAsync(message);
    }
}