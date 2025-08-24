using Bot;
using Bot.Utility;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Services
{
    public class InteractionHandlingService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;
        private readonly Config _config;
        private readonly ILogger<InteractionService> _logger;
        private readonly IHostEnvironment _environment;

        public InteractionHandlingService(
            DiscordSocketClient discord,
            InteractionService interactions,
            IServiceProvider services,
            IOptions<Config> config,
            ILogger<InteractionService> logger,
            IHostEnvironment environment)
        {
            _discord = discord;
            _interactions = interactions;
            _services = services;
            _config = config.Value;
            _logger = logger;

            _interactions.Log += msg => LogHelper.OnLogAsync(_logger, msg);
            _environment = environment;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _discord.Ready += OnReadyAsync;
            _discord.InteractionCreated += OnInteractionAsync;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _interactions.Dispose();
            return Task.CompletedTask;
        }

        public async Task OnInteractionAsync(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_discord, interaction);
                var result = await _interactions.ExecuteCommandAsync(context, _services);

                if (!result.IsSuccess)
                    await context.Channel.SendMessageAsync(result.ToString());
            }
            catch
            {
                if (interaction.Type == InteractionType.ApplicationCommand)
                {
                    await interaction.GetOriginalResponseAsync()
                        .ContinueWith(msg => msg.Result.DeleteAsync());
                }
            }
        }

        private async Task OnReadyAsync()
        {
            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            if (_environment.IsDevelopment())
            {
                var guildId = _config.TestServerId;

                await _interactions.RegisterCommandsToGuildAsync(guildId, true);
                _logger.LogInformation("Registered commands to guild: " + guildId);
            }
            else if (_environment.IsProduction())
            {
                await _interactions.RegisterCommandsGloballyAsync(true);
                _logger.LogInformation("Registered commands globally");
            }
        }

    }
}