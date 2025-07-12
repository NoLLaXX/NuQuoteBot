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
        [SlashCommand("removefrommoderlist", "Снять роль модератора")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveFromModerList(IGuildUser user)
        {
            var moderRoleId = await _guildService.GetModeratorRoleAsync(Context.Guild.Id);

            if (moderRoleId == null)
            {
                await RespondAsync("Роль модератора не установлена. Пожалуйста, установите роль с помощью команды `/setmoderrole`.", ephemeral: true);
                return;
            }

            await user.RemoveRoleAsync(moderRoleId.Value);

            await RespondAsync($"{user.Mention} больше не модератор", ephemeral: true);
        }
    }
}