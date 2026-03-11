using Application.Services;
using Application.Extensions;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using SharedConfiguration;

namespace Bot.Services
{
    public class QuoteSenderService : BackgroundService
    {
        private readonly ILogger<QuoteSenderService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly DiscordSocketClient _discord;
        private readonly Config _config;

        public QuoteSenderService(
            ILogger<QuoteSenderService> logger,
            IServiceProvider serviceProvider,
            DiscordSocketClient discord,
            IOptions<Config> config
            )
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
                    var guildService = scope.ServiceProvider.GetRequiredService<OurGuildService>();
                    var quoteService = scope.ServiceProvider.GetRequiredService<QuoteService>();
                    var discordUtilityService = scope.ServiceProvider.GetRequiredService<DiscordUtilityService>();

                    try
                    {
                        // �������� ��� ������ �� �������� Approved � MsgId == null
                        var quotes = await quoteService.GetApprovedQuotesWithNoMsgIdAsync();
                        foreach (var quote in quotes)
                        {
                            try
                            {
                                var guild = _discord.GetGuild(quote.OurGuildId);
                                if (guild == null) continue;
                                var channelId = await guildService.GetQuoteChannelAsync(quote.OurGuildId);
                                if (channelId == null) continue;
                                var channel = guild.GetTextChannel(channelId.Value);
                                if (channel == null) continue;

                                // ��������� ������ � �����

                                string? nick = null;
                                if (!quote.IsAnon) nick = discordUtilityService.GetGuildUserNick(quote.OurGuildId, quote.OurMemberId);

                                var formattedText = quote.Text.ToQuoteBox(nick);
                                var msg = await channel.SendMessageAsync(formattedText);
                                await quoteService.SetMsgIdAsync(quote.Id, msg.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"������ ��� �������� ������ {quote.Id}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "������ � QuoteSenderService");
                    }
                }

                Console.WriteLine($"QuoteSenderService: ��������� ������ ����������.");

                await Task.Delay(TimeSpan.FromSeconds(_config.SecsToCheckDB), stoppingToken);
            }
        }
    }
}