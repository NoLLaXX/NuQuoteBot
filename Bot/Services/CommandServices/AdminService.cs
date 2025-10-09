using Application.Services;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.CommandServices
{
    public class AdminService
    {
        private OurGuildService _guildService;
        //private QuoteService _quoteService;
        //private OurMemberService _memberService;
        //private DiscordUtilityService _discordUtilityService;

        public AdminService(
            OurGuildService guildService,
            QuoteService quoteService,
            OurMemberService memberService,
            DiscordUtilityService discordUtilityService)
        {
            _guildService = guildService;
            //_quoteService = quoteService;
            //_memberService = memberService;
            //_discordUtilityService = discordUtilityService;
        }

        public async Task AddToModerList(IGuildUser user, ulong guildId)
        {
            var moderRoleId = await _guildService.GetModeratorRoleAsync(guildId);

            if (moderRoleId == null)
            {
                throw new InvalidOperationException("Moder role is not set");
            }

            await user.AddRoleAsync(moderRoleId.Value);

        }

        public async Task RemoveFromModerList(IGuildUser user, ulong guildId)
        {
            var moderRoleId = await _guildService.GetModeratorRoleAsync(guildId);

            if (moderRoleId == null)
            {
                throw new InvalidOperationException("moder role is not set");
            }

            await user.RemoveRoleAsync(moderRoleId.Value);
        }


        public async Task SetModerRole(ulong roleId, ulong guildId)
        {
            await _guildService.SetModeratorRoleAsync(guildId, roleId);
        }

        public async Task SetQuoteChannel(ulong chanelId, ulong guildId)
        {
            await _guildService.SetQuoteChannelAsync(guildId, chanelId);
        }

    }
}
