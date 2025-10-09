using Application.Services;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.GeneralCommands
{
    public partial class GeneralCommands
    {
        [SlashCommand("alo", "позвать бота")]
        public async Task Alo()
        {
            await RespondAsync("ало", ephemeral: true);
        }
    }
}