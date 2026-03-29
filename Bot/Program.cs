using Application;
using Bot.Services;
using Bot.Services.CommandServices;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Example.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using SharedConfiguration;
using DataBaseInfrastructure;

namespace Bot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.Sources.Clear();

            var config = ConfigurationFactory.BuildConfiguration();

            builder.Services.AddAppConfiguration(config);
            builder.Services.AddApplicationServices(config.GetConnectionString("DefaultConnection"));

            builder.Services.AddCommandServices();

            // Регистрация DiscordSocketClient и InteractionService
            builder.Services.AddSingleton<DiscordSocketClient>(sp =>
            {
                var config = new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.All,
                };
                return new DiscordSocketClient(config);
            });

            builder.Services.AddSingleton<InteractionService>(sp =>
            {
                var client = sp.GetRequiredService<DiscordSocketClient>();
                return new InteractionService(client);
            });

            builder.Services.AddHostedService<InteractionHandlingService>();
            builder.Services.AddHostedService<DiscordStartupService>();

            //builder.Services.AddHostedService<QuoteCleanupService>();
            //builder.Services.AddHostedService<QuoteSenderService>();

            builder.Services.AddScoped<DiscordUtilityService>();

            var host = builder.Build();

            await host.ApplyMigrations();

            await host.RunAsync();
        }


        
    }
}