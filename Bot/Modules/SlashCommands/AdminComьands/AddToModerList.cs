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
        [SlashCommand("addtomoderlist", "Сделать модератором")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddToModerList(IGuildUser user)
        {
            try
            {
                await this._adminService.AddToModerList(user, Context.Guild.Id);
                await RespondAsync($"{user.Mention} Теперь модератор", ephemeral: true);
            }
            catch (InvalidOperationException ex)
            {
                await RespondAsync(ex.Message, ephemeral: true);
            }
        }
    }
}