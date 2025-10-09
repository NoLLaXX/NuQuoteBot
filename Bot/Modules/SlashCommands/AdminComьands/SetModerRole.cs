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

        [SlashCommand("setmoderrole", "Установить роль модератора цитат")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetModerRole(IRole role)
        {
            await RespondAsync($"Роль для модератора установлена: {role.Mention}", ephemeral: true);
        }
    }
}