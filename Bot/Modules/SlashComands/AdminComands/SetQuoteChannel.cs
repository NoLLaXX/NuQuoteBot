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

        [SlashCommand("setquotechannel", "Установить канал для цитат")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetQuoteChannel(ITextChannel channel)
        {
            var res = await _guildService.SetQuoteChannelAsync(Context.Guild.Id, channel.Id);
            if (!res)
                await RespondAsync("Не удалось установить канал для цитат. Проверьте права бота и существование канала.", ephemeral: true);
            else
                await RespondAsync($"Канал для цитат установлен: {channel.Mention}", ephemeral: true);
        }
    }
}