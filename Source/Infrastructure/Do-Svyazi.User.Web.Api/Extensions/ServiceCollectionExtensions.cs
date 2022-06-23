using System.Text;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Authenticate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenApiInfo = Microsoft.OpenApi.Models.OpenApiInfo;
using OpenApiSecurityRequirement = Microsoft.OpenApi.Models.OpenApiSecurityRequirement;
using OpenApiSecurityScheme = Microsoft.OpenApi.Models.OpenApiSecurityScheme;

namespace Do_Svyazi.User.Web.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<MessageIdentityUser, MessageIdentityRole>(options =>
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
                ValidateAudience = true,
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

            opt.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Do Svyazi",
                Version = "v1",
                Description = "Do Svyazi API",
            });

            opt.AddSecurityDefinition("Bearer (value: SecretKey)", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer (value: SecretKey)",
                            Type = ReferenceType.SecurityScheme,
                        },
                    },
                    new List<string>()
                },
            });
        });

        return services;
    }
}