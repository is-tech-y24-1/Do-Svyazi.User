using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Dtos.Mapping;
using Do_Svyazi.User.Web.Api.Extensions;
using Do_Svyazi.User.Web.Controllers.Middlewares;
using Do_Svyazi.User.Web.Controllers.Tools;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Do_Svyazi.User.Web.Api;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; init; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddDo_SvyaziControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services
            .AddDomainServices(Configuration)
            .AddAuthServices(Configuration)
            .AddSwaggerServices()
            .AddEndpointsApiExplorer();

        services.AddMediatR(typeof(GetUsersQuery).Assembly);
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    [Obsolete("Obsolete")]
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseSwagger(settings => { settings.RouteTemplate = "/swagger/{documentName}/swagger.json"; });
        app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v2.1.0/swagger.json", "Do-Svyazi User API"));

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<AuthorizationMiddleware>();
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}