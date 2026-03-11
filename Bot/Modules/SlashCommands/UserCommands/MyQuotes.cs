using Application.DTO;
using Discord;
using Discord.Interactions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.UserCommands
{
    public partial class UserCommands
    {
        [SlashCommand("myquotes", "Показать ваши цитаты")]
        public async Task MyQuotes(int page = 0, string status = "All")
        {
            List<QuoteDto> quotes = status switch
            {
                "Pending" => await _userService.GetPendingQuotesByUserAsync((ulong)Context.Guild.Id, (ulong)Context.User.Id, skip: page * 5, take: 5),
                "Approved" => await _userService.GetApprovedQuotesByUserAsync((ulong)Context.Guild.Id, (ulong)Context.User.Id, skip: page * 5, take: 5),
                "Denied" => await _userService.GetDeniedQuotesByUserAsync((ulong)Context.Guild.Id, (ulong)Context.User.Id, skip: page * 5, take: 5),
                _ => await _userService.GetQuotesByUserAsync((ulong)Context.Guild.Id, (ulong)Context.User.Id, skip: page * 5, take: 5)
            };

            var embed = new EmbedBuilder()
                .WithTitle($"Ваши цитаты (стр. {page + 1}, фильтр: {status})")
                .WithDescription(quotes.Count == 0 ? "Нет цитат." : string.Join("\n\n", quotes.Select(q => $"**ID:** {q.Id}\n{q.Text}")))
                .Build();

            var builder = new ComponentBuilder()
                .WithButton("⬅️", $"myquotes_prev_{status}_{page}", disabled: page == 0)
                .WithButton("➡️", $"myquotes_next_{status}_{page}", disabled: quotes.Count < 5)
                .WithButton("Все", $"myquotes_filter_All_{page}", ButtonStyle.Secondary, disabled: status == "All")
                .WithButton("На рассмотрении", $"myquotes_filter_Pending_{page}", ButtonStyle.Secondary, disabled: status == "Pending")
                .WithButton("Одобренные", $"myquotes_filter_Approved_{page}", ButtonStyle.Secondary, disabled: status == "Approved")
                .WithButton("Отклонённые", $"myquotes_filter_Denied_{page}", ButtonStyle.Secondary, disabled: status == "Denied");

            await RespondAsync(embed: embed, components: builder.Build(), ephemeral: true);
        }

        // Обработчики кнопок
        [ComponentInteraction("myquotes_prev_*_*")]
        public async Task MyQuotesPrev(string status, string page)
        {
            if (!int.TryParse(page, out int pageNumber)) pageNumber = 0;

            Console.WriteLine($"[myquotes_prev] status: {status}, page: {pageNumber}");
            await MyQuotes(pageNumber - 1, status);
        }

        [ComponentInteraction("myquotes_next_*_*")]
        public async Task MyQuotesNext(string status, string page)
        {
            if (!int.TryParse(page, out int pageNumber)) pageNumber = 0;

            Console.WriteLine($"[myquotes_next] status: {status}, page: {pageNumber}");
            await MyQuotes(pageNumber + 1, status);
        }

        [ComponentInteraction("myquotes_filter_*_*")]
        public async Task MyQuotesFilter(string status, string page)
        {
            if (!int.TryParse(page, out int pageNumber)) pageNumber = 0;

            Console.WriteLine($"[myquotes_filter] status: {status}, page: {pageNumber}");
            await MyQuotes(0, status); // при смене фильтра всегда сбрасываем на первую страницу
        }
    }
}