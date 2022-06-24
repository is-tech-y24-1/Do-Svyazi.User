using Do_Svyazi.User.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Do_Svyazi.User.Web.Controllers.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Do_Svyazi_User_BusinessLogicException e)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Do_Svyazi_User_NotFoundException e)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(e.Message);
        }
    }
}