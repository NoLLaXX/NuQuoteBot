using Application;
using Bot.Services;
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

            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Development.json");
            }
            else if (builder.Environment.IsProduction())
            {
                builder.Configuration
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile(Path.Combine(new string[] { Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) , "Bot.appsettings.Development.json" }));
            }

            builder.Services.Configure<Config>(builder.Configuration.GetSection("MapSettings"));

            builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));

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