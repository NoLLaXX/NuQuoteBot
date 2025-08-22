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

        [SlashCommand("addtomoderlist", "Сделать модератором")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddToModerList(IGuildUser user)
        {
            var moderRoleId = await _guildService.GetModeratorRoleAsync(Context.Guild.Id);

            Console.WriteLine($"!!!! Guild ID: {Context.Guild.Id}");
            Console.WriteLine($"!!!! Moderator Role ID: {moderRoleId}");

            if (moderRoleId == null)
            {
                await RespondAsync("Роль модератора не установлена. Пожалуйста, установите роль с помощью команды `/setmoderrole`.", ephemeral: true);
                return;
            }

            await user.AddRoleAsync(moderRoleId.Value);

            await RespondAsync($"{user.Mention} Теперь модератор", ephemeral: true);
        }
    }
}