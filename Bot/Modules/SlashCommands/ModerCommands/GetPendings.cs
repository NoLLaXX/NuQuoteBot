using Discord;
using Discord.Interactions;

namespace Bot.Modules.SlashCommands.ModerCommands
{
    public partial class ModerCommands
    {
        [SlashCommand("getpendings", "Получить предложку")]
        public async Task GetPendings(string page = "0")
        {
            if (!int.TryParse(page, out int pageNum) || pageNum < 0) pageNum = 0;

            bool isModer = await _moderService.IsUserModeratorAsync(Context.Guild.Id, Context.User.Id);
            if (!isModer)
            {
                await RespondAsync("Только модераторы могут просматривать предложения.", ephemeral: true);
                return;
            }

            var quotes = await _moderService.GetPendingQuotesByGuildAsync((ulong)Context.Guild.Id, skip: pageNum, take: 1);
            var quote = quotes.FirstOrDefault();

            if (quote == null)
            {
                await RespondAsync("Нет предложенных цитат.", ephemeral: true);
                return;
            }

            var embed = new EmbedBuilder()
                .WithTitle($"Цитата на рассмотрении (стр. {pageNum + 1})")
                .WithDescription($"**ID:** {quote.Id}\n{quote.Text}")
                .Build();

            var builder = new ComponentBuilder()
                .WithButton("⬅️", $"pending_prev_{pageNum}", disabled: pageNum == 0)
                .WithButton("➡️", $"pending_next_{pageNum}")
                .WithButton("Одобрить", $"pending_approve_{quote.Id}_{pageNum}", ButtonStyle.Success)
                .WithButton("Отклонить", $"pending_deny_{quote.Id}_{pageNum}", ButtonStyle.Danger);

            await RespondAsync(embed: embed, components: builder.Build(), ephemeral: true);
        }

        [ComponentInteraction("pending_prev_*")]
        public async Task PendingPrev(string page)
        {
            if (!int.TryParse(page, out int pageNum)) pageNum = 0;
            await GetPendings((pageNum - 1).ToString());
        }

        [ComponentInteraction("pending_next_*")]
        public async Task PendingNext(string page)
        {
            if (!int.TryParse(page, out int pageNum)) pageNum = 0;
            await GetPendings((pageNum + 1).ToString());
        }

        [ComponentInteraction("pending_approve_*_*")]
        public async Task PendingApprove(string quoteId, string page)
        {
            if (ulong.TryParse(quoteId, out ulong id) && int.TryParse(page, out int pageNum))
            {
                try
                {
                    await _moderService.ApproveQuoteAsync(id);
                    await RespondAsync("Цитата одобрена", ephemeral: true);
                }
                catch (Exception ex)
                {
                    await RespondAsync($"Ошибка при одобрении цитаты: {ex.Message}", ephemeral: true);
                }
            }
        }

        [ComponentInteraction("pending_deny_*_*")]
        public async Task PendingDeny(string quoteId, string page)
        {
            if (ulong.TryParse(quoteId, out ulong id) && int.TryParse(page, out int pageNum))
            {
                try
                {
                    await _moderService.DenyQuoteAsync(id);
                    await RespondAsync("Цитата отклонена", ephemeral: true);
                }
                catch (Exception ex)
                {
                    await RespondAsync($"Ошибка при отклонении цитаты: {ex.Message}", ephemeral: true);
                }
            }
        }
    }
}