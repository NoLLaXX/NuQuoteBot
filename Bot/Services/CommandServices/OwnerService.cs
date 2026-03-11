using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SharedConfiguration;
using System.Threading.Tasks;

namespace Bot.Services.CommandServices
{
    public class OwnerService
    {
        private readonly InteractionService _interactions;
        private readonly Config _config;
        private readonly IHostEnvironment _environment;
        private readonly DiscordSocketClient _discord;
        private readonly ILogger<OwnerService> _logger;

        public OwnerService(
            InteractionService interactions,
            DiscordSocketClient discord,
            IOptions<Config> config,
            IHostEnvironment environment,
            ILogger<OwnerService> logger)
        {
            _interactions = interactions;
            _discord = discord;
            _config = config.Value;
            _environment = environment;
            _logger = logger;
        }

        public async Task ReRegisterSlashCommandsAsync()
        {

            var commands = await _discord.GetGlobalApplicationCommandsAsync();
            foreach (var c in commands)
            {
                await c.DeleteAsync();
            }

            var guildId = _config.TestServerId;

            if (_environment.IsDevelopment() && guildId != 0)
            {
                await _interactions.RegisterCommandsToGuildAsync(guildId, true);
                _logger.LogCritical("Registered commands to guild: " + guildId);
            }
            else
            {
                await _interactions.RegisterCommandsGloballyAsync(true);
                _logger.LogCritical("Registered commands globally");
            }
        }
    }
}