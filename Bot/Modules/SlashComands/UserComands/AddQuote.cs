using Application.Services;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashComands
{
    public partial class TestSlashCommands : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("addquote", "Добавить цитату")]
        public async Task AddQuote(
            string line1,
            string? line2 = null,
            string? line3 = null,
            string? line4 = null,
            string? line5 = null,
            string? line6 = null,
            string? line7 = null,
            string? line8 = null,
            string? line9 = null,
            string? line10 = null,
            bool isAnon = false)
        {
            try
            {
                var lines = new[] { line1, line2, line3, line4, line5, line6, line7, line8, line9, line10 }
                    .Where(s => !string.IsNullOrWhiteSpace(s));
                var fullText = string.Join('\n', lines);

                await _quoteService.AddQuoteAsync(Context.User.Id, Context.Guild.Id, fullText, isAnon);
                await RespondAsync("Цитата добавлена");
            }
            catch
            {
                await RespondAsync("Неправильно введён текст");
            }
        }
    }
}