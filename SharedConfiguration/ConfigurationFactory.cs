using System;
using Microsoft.Extensions.Configuration;

namespace SharedConfiguration;

    public static class ConfigurationFactory
    {
        public static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }
    }