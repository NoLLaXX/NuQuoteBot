using Application.Services;
using Application.Services;
using Bot.Services;
using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands
{
    public partial class TestSlashCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly OurGuildService _guildService;
        private readonly QuoteService _quoteService;
        private readonly OurMemberService _memberService;
        private readonly DiscordUtilityService _discordUtilityService;

        public TestSlashCommands(OurGuildService guildService, QuoteService quoteService, OurMemberService memberService, DiscordUtilityService discordUtilityService)
        {
            _guildService = guildService;
            _quoteService = quoteService;
            _memberService = memberService;
            _discordUtilityService = discordUtilityService;
        }
    }
}
