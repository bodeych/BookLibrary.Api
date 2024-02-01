using BookLibrary.Api.Application.Interfaces;
using BookLibrary.Api.Infrasrtucture.Repositories;
using BookLibrary.Api.Infrasrtucture.Settings;

namespace BookLibrary.Api.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }

    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DatabaseSettings>(_ =>
        {
            var databaseSettings = new DatabaseSettings();

            configuration.GetSection(nameof(DatabaseSettings)).Bind(databaseSettings);

            return databaseSettings;
        });

        return services;
    }
}