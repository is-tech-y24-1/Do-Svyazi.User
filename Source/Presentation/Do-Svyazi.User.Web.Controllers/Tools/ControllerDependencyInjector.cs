using Microsoft.Extensions.DependencyInjection;

namespace Do_Svyazi.User.Web.Controllers.Tools;

public static class ControllerDependencyInjector
{
    public static IMvcBuilder AddDo_SvyaziControllers(this IMvcBuilder builder) =>
        builder.AddApplicationPart(typeof(ControllerDependencyInjector).Assembly);
}