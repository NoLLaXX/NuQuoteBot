using Application.DTO;
using Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Services.CommandServices
{
    public class UserService
    {
        private readonly QuoteService _quoteService;
        private readonly DiscordUtilityService _discordUtilityService;

        public UserService(
            OurGuildService guildService,
            QuoteService quoteService,
            OurMemberService memberService,
            DiscordUtilityService discordUtilityService)
        {
            _quoteService = quoteService;
            _discordUtilityService = discordUtilityService;
        }

        public Task AddQuoteAsync(ulong userId, ulong guildId, string text, bool isAnon) =>
            _quoteService.AddQuoteAsync(userId, guildId, text, isAnon);

        public Task<QuoteDto?> FindAsync(ulong quoteId) =>
            _quoteService.FindAsync(quoteId);

        public Task<bool> MarkQuoteOnDeletionAsync(ulong quoteId) =>
            _quoteService.MarkQuoteOnDeletionAsync(quoteId);

        public Task<bool> IsUserModeratorAsync(ulong guildId, ulong userId) =>
            _discordUtilityService.IsUserModeratorAsync(guildId, userId);

        public Task<List<QuoteDto>> GetQuotesByUserAsync(ulong guildId, ulong userId, int skip = 0, int? take = null) =>
            _quoteService.GetQuotesByUserAsync(guildId, userId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetPendingQuotesByUserAsync(ulong guildId, ulong userId, int skip = 0, int? take = null) =>
            _quoteService.GetPendingQuotesByUserAsync(guildId, userId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetApprovedQuotesByUserAsync(ulong guildId, ulong userId, int skip = 0, int? take = null) =>
            _quoteService.GetApprovedQuotesByUserAsync(guildId, userId, skip: skip, take: take);

        public Task<List<QuoteDto>> GetDeniedQuotesByUserAsync(ulong guildId, ulong userId, int skip = 0, int? take = null) =>
            _quoteService.GetDeniedQuotesByUserAsync(guildId, userId, skip: skip, take: take);
    }
}
