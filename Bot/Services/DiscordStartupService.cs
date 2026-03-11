using Bot;
using Bot.Utility;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedConfiguration;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Services
{
    public class DiscordStartupService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly Config _config;
        private readonly ILogger<DiscordSocketClient> _logger;

        public DiscordStartupService(DiscordSocketClient discord, IOptions<Config> config, ILogger<DiscordSocketClient> logger)
        {
            _discord = discord;
            _config = config.Value;
            _logger = logger;

            _discord.Log += msg => LogHelper.OnLogAsync(_logger, msg);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _discord.LoginAsync(TokenType.Bot, _config.Token);
            await _discord.StartAsync();

            _discord.Ready += async () =>
            {

                _logger.LogInformation("Discord client is ready.");
                await _discord.SetStatusAsync(UserStatus.Online);
                await _discord.SetActivityAsync(new Game("C-OPS", ActivityType.Playing));
            };
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discord.LogoutAsync();
            await _discord.StopAsync();
        }
    }
}
