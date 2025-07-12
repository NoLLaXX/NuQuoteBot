using Application.Services;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bot.Services
{
    public class QuoteCleanupService : BackgroundService
    {
        private readonly ILogger<QuoteCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discord;
        private readonly Config _config;

        public QuoteCleanupService(
            ILogger<QuoteCleanupService> logger,
            IServiceProvider serviceProvider,
            DiscordSocketClient discord,
            IOptions<Config> config)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _discord = discord;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var quoteService = scope.ServiceProvider.GetRequiredService<QuoteService>();
                    var guildService = scope.ServiceProvider.GetRequiredService<OurGuildService>();

                    try
                    {
                        var quotes = await quoteService.GetOnDeletionQuotesByGuildAsync();
                        foreach (var quote in quotes)
                        {
                            try
                            {
                                var guild = _discord.GetGuild(quote.OurGuildId);
                                if (guild == null) continue;
                                var channelId = await guildService.GetQuoteChannelAsync(quote.OurGuildId);
                                if (channelId == null) continue;
                                var channel = guild.GetTextChannel(channelId.Value);
                                if (channel == null || quote.MsgId == null) continue;
                                var msg = await channel.GetMessageAsync(quote.MsgId.Value);
                                if (msg != null)
                                    await msg.DeleteAsync();
                                await quoteService.MarkQuoteDeletedAsync(quote.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Ошибка при удалении сообщения цитаты {quote.Id}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка в QuoteCleanupService");
                    }
                }

                Console.WriteLine($"QuoteCleanupService: Проверка цитат на удаление завершена. Цитаты удалены.");
                await Task.Delay(TimeSpan.FromSeconds(_config.SecsToCheckDB), stoppingToken);
            }
        }
    }
}
