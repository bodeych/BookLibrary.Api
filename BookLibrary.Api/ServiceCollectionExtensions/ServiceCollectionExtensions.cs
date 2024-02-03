using System.Text;
using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Application.Service;
using BookLibrary.Api.Application.Settings;
using BookLibrary.Api.Infrastructure.Repositories;
using BookLibrary.Api.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BookLibrary.Api.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }

    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(nameof(jwtSettings), jwtSettings);

        services.AddScoped<DatabaseSettings>(_ =>
        {
            var databaseSettings = new DatabaseSettings();

            configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            return databaseSettings;
        });

        services.AddScoped<JwtSettings>(_ =>
        {
            var jwtSettings = new JwtSettings();

            configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

            return jwtSettings;
        });

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
        services.AddSingleton(tokenValidationParameters);
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                // for development only
                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
            });

        return services;
    }
    
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GenerateToken>();
        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
        
        
        return services;
    }
}