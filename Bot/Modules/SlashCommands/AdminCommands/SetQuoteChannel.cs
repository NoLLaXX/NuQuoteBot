using Application.Services;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.AdminCommands
{
    public partial class AdminCommands
    {
        [SlashCommand("setquotechannel", "Установить канал для цитат")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetQuoteChannel(ITextChannel channel)
        {
            await this._adminService.SetQuoteChannel(channel.Id, Context.Guild.Id);
            await RespondAsync($"Канал для цитат установлен: {channel.Mention}", ephemeral: true);
        }
    }
}