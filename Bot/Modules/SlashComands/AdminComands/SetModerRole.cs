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

        [SlashCommand("setmoderrole", "Установить роль модератора цитат")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SetModerRole(IRole role)
        {
            var res = await _guildService.SetModeratorRoleAsync(Context.Guild.Id, role.Id);
            if (!res)
                await RespondAsync("Не удалось установить роль для модератора. Проверьте права бота и существование роли.", ephemeral: true);
            else
                await RespondAsync($"Роль для модератора установлена: {role.Mention}", ephemeral: true);
        }
    }
}