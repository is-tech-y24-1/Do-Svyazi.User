using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Dtos.Mapping;
using Do_Svyazi.User.Web.Controllers;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Do_Svyazi.User.Web.Api;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; init; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddDo_SvyaziControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddDbContext<IUsersAndChatDbContext, UsersAndUsersAndChatDbContext>(optionsBuilder =>
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("Database"));
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerDocument(settings =>
        {
            settings.Title = "Do-Svyazi.User";
            settings.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "Do-Svyazi.User";
                document.Info.Description = "Do-Svyazi.User module";
            };
        });

        services.AddMediatR(typeof(GetUsers).Assembly);
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();

        app.UseOpenApi();
        app.UseSwaggerUi3(c =>
        {
            c.DocumentTitle = "Do-Svyazi.User";
            c.DocumentPath = "/swagger/v1/swagger.json";
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}