using System.Text;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Do_Svyazi.User.Web.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<MessengerUser, MessageIdentityRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequireDigit = true,
                    RequireNonAlphanumeric = false,
                    RequireUppercase = true,
                    RequireLowercase = true,
                    RequiredLength = 6,
                };

                options.User = new UserOptions
                {
                    RequireUniqueEmail = true,
                };
            })
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
                ValidateAudience = false,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
            };
        });

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IDbContext, DoSvaziDbContext>(optionsBuilder =>
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite(configuration.GetConnectionString("Database"));
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.EnableSensitiveDataLogging();
            options.UseSqlite(configuration.GetConnectionString("Identity"));
        });

        return services;
    }

    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.UseInlineDefinitionsForEnums();

            opt.SwaggerDoc("v2.1.0", new OpenApiInfo
            {
                Title = "Do-Svyazi User module",
                Version = "v2.1.0",
                Description = "Do Svyazi User API",
            });

            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                },
            };

            opt.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    securityScheme,
                    new List<string>()
                },
            });
        });

        return services;
    }
}