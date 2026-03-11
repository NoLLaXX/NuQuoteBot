using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SharedConfiguration;

public static class AddConfiguration
{
    public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services
            .Configure<Config>(config.GetSection("Config"));

        return services;
    }
}
