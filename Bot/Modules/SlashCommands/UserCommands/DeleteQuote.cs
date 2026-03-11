using Application.Extensions;
using Discord;
using Discord.Interactions;

namespace Bot.Modules.SlashCommands.UserCommands
{
    public partial class UserCommands
    {
        [SlashCommand("deletequote", "Удалить цитату по ID")]
        public async Task DeleteQuote(ulong quoteId)
        {
            // Получаем цитату
            var quote = await _userService.FindAsync(quoteId);
            if (quote == null)
            {
                await RespondAsync($"Цитата с ID {quoteId} не найдена", ephemeral: true);
                return;
            }

            // Если цитата принадлежит пользователю
            if (quote.OurMemberId == Context.User.Id)
            {
                var result = await _userService.MarkQuoteOnDeletionAsync(quoteId);
                if (result)
                    await RespondAsync($"Цитата {quoteId} удалена", ephemeral: true);
                else
                    await RespondAsync($"Не удалось удалить цитату {quoteId}", ephemeral: true);
                return;
            }

            // Проверяем, является ли пользователь модератором
            bool isModer = await _userService.IsUserModeratorAsync(Context.Guild.Id, Context.User.Id);
            if (isModer)
            {
                var result = await _userService.MarkQuoteOnDeletionAsync(quoteId);
                if (result)
                    await RespondAsync($"Цитата {quoteId} удалена модератором", ephemeral: true);
                else
                    await RespondAsync($"Не удалось удалить цитату {quoteId}", ephemeral: true);
            }
            else
            {
                await RespondAsync($"Вы не можете удалить чужую цитату (ID {quoteId})", ephemeral: true);
            }
        }
    }
}