using Application.DTO;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules.SlashComands
{
    public partial class TestSlashCommands : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("allquotes", "Показать все цитаты сервера (только для модераторов)")]
        public async Task AllQuotes(int page = 0, string status = "All")
        {
            bool isModer = await _discordUtilityService.IsUserModeratorAsync(Context.Guild.Id, Context.User.Id);
            if (!isModer)
            {
                await RespondAsync("Только модераторы могут просматривать все цитаты сервера.", ephemeral: true);
                return;
            }

            List<QuoteDto> quotes = status switch
            {
                "Pending" => await _quoteService.GetPendingQuotesByGuildAsync((ulong)Context.Guild.Id, skip: page * 5, take: 5),
                "Approved" => await _quoteService.GetApprovedQuotesByGuildAsync((ulong)Context.Guild.Id, skip: page * 5, take: 5),
                "Denied" => await _quoteService.GetDeniedQuotesByGuildAsync((ulong)Context.Guild.Id, skip: page * 5, take: 5),
                _ => await _quoteService.GetQuotesByGuildAsync((ulong)Context.Guild.Id, skip: page * 5, take: 5)
            };

            var embed = new EmbedBuilder()
                .WithTitle($"Все цитаты сервера (стр. {page + 1}, фильтр: {status})")
                .WithDescription(quotes.Count == 0 ? "Нет цитат." : string.Join("\n\n", quotes.Select(q => $"**ID:** {q.Id}\n{q.Text}")))
                .Build();

            var builder = new ComponentBuilder()
                .WithButton("⬅️", $"allquotes_prev_{status}_{page}", disabled: page == 0)
                .WithButton("➡️", $"allquotes_next_{status}_{page}", disabled: quotes.Count < 5)
                .WithButton("Все", $"allquotes_filter_All_{page}", ButtonStyle.Secondary, disabled: status == "All")
                .WithButton("На рассмотрении", $"allquotes_filter_Pending_{page}", ButtonStyle.Secondary, disabled: status == "Pending")
                .WithButton("Одобренные", $"allquotes_filter_Approved_{page}", ButtonStyle.Secondary, disabled: status == "Approved")
                .WithButton("Отклонённые", $"allquotes_filter_Denied_{page}", ButtonStyle.Secondary, disabled: status == "Denied");

            await RespondAsync(embed: embed, components: builder.Build(), ephemeral: true);
        }

        // Обработчики кнопок для allquotes
        [ComponentInteraction("allquotes_prev_*_*")]
        public async Task AllQuotesPrev(string status, string page)
        {
            if (!int.TryParse(page, out int pageNumber)) pageNumber = 0;
            await AllQuotes(pageNumber - 1, status);
        }

        [ComponentInteraction("allquotes_next_*_*")]
        public async Task AllQuotesNext(string status, string page)
        {
            if (!int.TryParse(page, out int pageNumber)) pageNumber = 0;
            await AllQuotes(pageNumber + 1, status);
        }

        [ComponentInteraction("allquotes_filter_*_*")]
        public async Task AllQuotesFilter(string status, string page)
        {
            await AllQuotes(0, status);
        }
    }
}
