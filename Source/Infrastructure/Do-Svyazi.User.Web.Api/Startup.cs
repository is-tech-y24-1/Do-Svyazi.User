using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Dtos.Mapping;
using Do_Svyazi.User.Web.Controllers.Tools;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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

        services.AddDbContext<IDbContext, DoSvaziDbContext>(optionsBuilder =>
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("Database"));
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlite(Configuration.GetConnectionString("Identity"));
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddIdentity<MessageIdentityUser, MessageIdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration["JWT:ValidAudience"],
                ValidIssuer = Configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
            };
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
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseOpenApi();
        app.UseSwaggerUi3(c =>
        {
            c.DocumentTitle = "Do-Svyazi.User";
            c.DocumentPath = "/swagger/v1/swagger.json";
        });

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}