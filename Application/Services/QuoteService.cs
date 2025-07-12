using Application.DTO;
using DataBaseInfrastructure.Enums;
using DataBaseInfrastructure.Models;
using DataBaseInfrastructure.Repository;
using DataBaseInfrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Extensions;

namespace Application.Services
{
    public class QuoteService
    {
        private readonly QuoteRepository _quoteRepository;
        private readonly DataIntegrityService _dataIntegrityService;

        public QuoteService(QuoteRepository quoteRepository, DataIntegrityService dataIntegrityService)
        {
            _quoteRepository = quoteRepository;
            _dataIntegrityService = dataIntegrityService;
        }

        public async Task<List<QuoteDto>> GetQuotesByGuildAsync(ulong guildId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            var statuses = new List<StatusOptions> { StatusOptions.Pending, StatusOptions.Approved, StatusOptions.Denied };
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, statuses, null, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetPendingQuotesByGuildAsync(ulong guildId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Pending }, null, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetApprovedQuotesByGuildAsync(ulong guildId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Approved }, null, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetDeniedQuotesByGuildAsync(ulong guildId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Denied }, null, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetOnDeletionQuotesByGuildAsync()
        {
            var quotes = await _quoteRepository.GetFiltered(statusList: new List<StatusOptions> { StatusOptions.OnDeletion });
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetQuotesByUserAsync(ulong guildId, ulong userId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            await _dataIntegrityService.EnsureMemberExistsAsync(userId);
            var statuses = new List<StatusOptions> { StatusOptions.Pending, StatusOptions.Approved, StatusOptions.Denied };
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, statuses, userId, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetPendingQuotesByUserAsync(ulong guildId, ulong userId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            await _dataIntegrityService.EnsureMemberExistsAsync(userId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Pending }, userId, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetApprovedQuotesByUserAsync(ulong guildId, ulong userId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            await _dataIntegrityService.EnsureMemberExistsAsync(userId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Approved }, userId, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task<List<QuoteDto>> GetDeniedQuotesByUserAsync(ulong guildId, ulong userId, bool ascending = false, int skip = 0, int? take = null)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);
            await _dataIntegrityService.EnsureMemberExistsAsync(userId);
            var quotes = await _quoteRepository.GetFiltered(guildId, take, skip, new List<StatusOptions> { StatusOptions.Denied }, userId, ascending);
            return quotes.Select(q => q.ToDto()).ToList();
        }

        public async Task AddQuoteAsync(ulong ourMemberId, ulong ourGuildId, string text, bool isAnon)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Quote text cannot be null or empty", nameof(text));

            await _dataIntegrityService.EnsureGuildExistsAsync(ourGuildId);
            await _dataIntegrityService.EnsureMemberExistsAsync(ourMemberId);
            await _quoteRepository.CreateAsync(ourMemberId, ourGuildId, text ?? string.Empty, isAnon);
        }

        public async Task<bool> MarkQuoteOnDeletionAsync(ulong quoteId)
        {
            var quote = await _quoteRepository.FindAsync(quoteId);
            if (quote == null)
                return false;

            return await _quoteRepository.ChangeStatusAsync(quoteId, StatusOptions.OnDeletion);
        }

        public async Task<bool> MarkQuoteDeletedAsync(ulong quoteId)
        {
            var quote = await _quoteRepository.FindAsync(quoteId);
            if (quote == null)
                return false;

            await _quoteRepository.SetMsgIdAsync(quoteId, null); // Clear message ID if it exists

            return await _quoteRepository.ChangeStatusAsync(quoteId, StatusOptions.Deleted);
        }

        public async Task<bool> DeleteQuoteAsync(ulong quoteId, ulong userId)
        {
            await _dataIntegrityService.EnsureMemberExistsAsync(userId);
            return await _quoteRepository.DeleteAsync(quoteId, userId);
        }

        public async Task<bool> ApproveQuoteAsync(ulong quoteId)
        {
            var quote = await _quoteRepository.FindAsync(quoteId);
            if (quote == null)
                throw new InvalidOperationException("Quote not found");
            if (quote.Status != StatusOptions.Pending)
                throw new InvalidOperationException("Status can only be changed from Pending to Approved");
            return await _quoteRepository.ChangeStatusAsync(quoteId, StatusOptions.Approved);
        }

        public async Task<bool> DenyQuoteAsync(ulong quoteId)
        {
            var quote = await _quoteRepository.FindAsync(quoteId);
            if (quote == null)
                throw new InvalidOperationException("Quote not found");
            if (quote.Status != StatusOptions.Pending)
                throw new InvalidOperationException("Status can only be changed from Pending to Denied");
            return await _quoteRepository.ChangeStatusAsync(quoteId, StatusOptions.Denied);
        }

        public async Task<bool> SetMsgIdAsync(ulong quoteId, ulong? messageId)
        {
            return await _quoteRepository.SetMsgIdAsync(quoteId, messageId);
        }

        public async Task<QuoteDto?> FindAsync(ulong quoteId)
        {
            var quote = await _quoteRepository.FindAsync(quoteId);
            return quote?.ToDto();
        }

        public async Task<List<QuoteDto>> GetApprovedQuotesWithNoMsgIdAsync()
        {
            var quotes = await _quoteRepository.GetApprovedQuotesWithNoMsgIdAsync();
            return quotes.Select(q => q.ToDto()).ToList();
        }
    }
}
