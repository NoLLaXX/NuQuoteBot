using Application.DTO;
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
        private readonly QuoteService _quoteService;
        private readonly DiscordUtilityService _discordUtilityService;

        public ModerService(
            OurGuildService guildService,
            QuoteService quoteService,
            OurMemberService memberService,
            DiscordUtilityService discordUtilityService)
        {
            _quoteService = quoteService;
            _discordUtilityService = discordUtilityService;
        }

        public Task<bool> IsUserModeratorAsync(ulong guildId, ulong userId) =>
            _discordUtilityService.IsUserModeratorAsync(guildId, userId);

        public Task<List<QuoteDto>> GetPendingQuotesByGuildAsync(ulong guildId, int skip = 0, int? take = null) =>
            _quoteService.GetPendingQuotesByGuildAsync(guildId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetApprovedQuotesByGuildAsync(ulong guildId, int skip = 0, int? take = null) =>
            _quoteService.GetApprovedQuotesByGuildAsync(guildId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetDeniedQuotesByGuildAsync(ulong guildId, int skip = 0, int? take = null) =>
            _quoteService.GetDeniedQuotesByGuildAsync(guildId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetQuotesByGuildAsync(ulong guildId, int skip = 0, int? take = null) =>
            _quoteService.GetQuotesByGuildAsync(guildId, skip: skip, take: take);

        public Task<bool> ApproveQuoteAsync(ulong quoteId) =>
            _quoteService.ApproveQuoteAsync(quoteId);

        public Task<bool> DenyQuoteAsync(ulong quoteId) =>
            _quoteService.DenyQuoteAsync(quoteId);
    }
}
