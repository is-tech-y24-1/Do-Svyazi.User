using Microsoft.Extensions.DependencyInjection;

namespace Do_Svyazi.User.Web.Controllers;

public static class ControllerDependencyInjector
{
    public static IMvcBuilder AddDo_SvyaziControllers(this IMvcBuilder builder)
    {
        return builder.AddApplicationPart(typeof(ControllerDependencyInjector).Assembly);
    }
}