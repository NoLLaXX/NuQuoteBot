using Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.CommandServices
{
    public class ModerService
    {
        private OurGuildService _guildService;
        private QuoteService _quoteService;
        private OurMemberService _memberService;
        private DiscordUtilityService _discordUtilityService;

        public ModerService(
            OurGuildService guildService,
            QuoteService quoteService,
            OurMemberService memberService,
            DiscordUtilityService discordUtilityService)
        {
            _guildService = guildService;
            _quoteService = quoteService;
            _memberService = memberService;
            _discordUtilityService = discordUtilityService;
        }
    }
}
