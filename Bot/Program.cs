using Application;
using Bot.Services;
using Bot.Services.CommandServices;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Example.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Configuration.Sources.Clear();

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile("appsettings.Development.json", true)
                    .AddUserSecrets<Program>()
                    .AddEnvironmentVariables();
            }
            else if (builder.Environment.IsProduction())
            {
                builder.Configuration
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Production.json")
                    .AddEnvironmentVariables();
            }

            builder.Services.Configure<Config>(builder.Configuration.GetSection("MapSettings"));

            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));
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

            builder.Services.AddHostedService<QuoteCleanupService>();
            builder.Services.AddHostedService<QuoteSenderService>();

            builder.Services.AddScoped<DiscordUtilityService>();

            var host = builder.Build();
            host.Run();
        }
    }
}