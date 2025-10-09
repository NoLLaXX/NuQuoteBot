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
        [SlashCommand("removefrommoderlist", "Снять роль модератора")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task RemoveFromModerList(IGuildUser user)
        {
            try
            {
                await _adminService.RemoveFromModerList(user, Context.Guild.Id);
                await RespondAsync($"{user.Mention} больше не модератор", ephemeral: true);
            }
            catch (InvalidOperationException ex)
            {
                await RespondAsync(ex.Message, ephemeral: true);
            }
        }
    }
}