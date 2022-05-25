using Do_Svyazi.User.Application.Abstractions.DbContexts;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
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

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
            .AddDo_SvyaziControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddDbContext<IUsersAndChatDbContext, UsersAndUsersAndChatDbContext>(optionsBuilder =>
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite("Data Source=aboba.db;Cache=Shared;");
        });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(opt =>
        {
            opt.UseInlineDefinitionsForEnums();
            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Do-Svyazi.User",
                Version = "v1",
            });
        });

        services.AddMediatR(typeof(GetUsers).Assembly, typeof(AddUser).Assembly);
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Do-Svyazi.User");
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}