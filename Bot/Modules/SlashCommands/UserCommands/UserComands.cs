using Application.Services;
using Bot.Services;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.UserCommands
{
    public partial class UserCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly OurGuildService _guildService;
        private readonly QuoteService _quoteService;
        private readonly OurMemberService _memberService;
        private readonly DiscordUtilityService _discordUtilityService;
    }
}
