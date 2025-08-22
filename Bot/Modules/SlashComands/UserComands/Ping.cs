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

        [SlashCommand("ping", "Ответить Pong!")]
        public async Task Ping()
        {
            await RespondAsync("Pong!", ephemeral: true);
        }
    }
}