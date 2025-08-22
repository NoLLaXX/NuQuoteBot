using DataBaseInfrastructure.Enums;
using DataBaseInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure.Repository
{
    public class QuoteRepository(MyDbContext dbContext)
    {
        public async Task<Quote?> FindAsync(ulong id)
        {
            var res = await dbContext.Quotes.FindAsync(id);
            return res;
        }

        public async Task<Quote> CreateAsync(ulong userId, ulong guildId, string text, bool isAnon)
        {
            var quote = new Quote
            {
                Text = text,
                IsAnon = isAnon,
                Created = DateTime.UtcNow,
                Status = StatusOptions.Pending,
                OurMemberId = userId,
                OurGuildId = guildId
            };

            dbContext.Quotes.Add(quote);
            await dbContext.SaveChangesAsync();
            return quote;
        }

        public async Task<List<Quote>> GetFiltered(ulong? guildId = null, int? take = null, int skip = 0, List<StatusOptions>? statusList = null, ulong? userId = null, bool ascending = false)
        {
            var query = dbContext.Quotes.AsQueryable();
            if (guildId.HasValue)
                query = query.Where(q => q.OurGuildId == guildId.Value);
            if (statusList != null)
                query = query.Where(q => statusList.Any(s => s == q.Status));
            if (userId.HasValue)
                query = query.Where(q => q.OurMemberId == userId.Value);
            query = ascending ? query.OrderBy(q => q.Created) : query.OrderByDescending(q => q.Created);
            if (skip > 0)
                query = query.Skip(skip);
            if (take.HasValue)
                query = query.Take(take.Value);
            return await query.ToListAsync();
        }

        public async Task<bool> DeleteAsync(ulong quoteId, ulong userId)
        {
            var quote = await dbContext.Quotes.FirstOrDefaultAsync(q => q.Id == quoteId && q.OurMemberId == userId);
            if (quote == null)
                return false;
            dbContext.Quotes.Remove(quote);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(ulong quoteId, StatusOptions status)
        {
            var quote = await dbContext.Quotes.FindAsync(quoteId);
            if (quote == null)
                return false;
            quote.Status = status;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetMsgIdAsync(ulong quoteId, ulong? msgId)
        {
            var quote = await dbContext.Quotes.FindAsync(quoteId);
            if (quote == null)
                return false;
            quote.MsgId = msgId;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Quote>> GetApprovedQuotesWithNoMsgIdAsync()
        {
            return await dbContext.Quotes
                .Where(q => q.Status == StatusOptions.Approved && q.MsgId == null)
                .ToListAsync();
        }
    }
}
