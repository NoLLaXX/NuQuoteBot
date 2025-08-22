using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseInfrastructure.Repository
{
    public class OurGuildRepository
    {
        private readonly MyDbContext _dbContext;
        public OurGuildRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OurGuild?> FindAsync(ulong guildId)
        {
            return await _dbContext.Guilds.FindAsync(guildId);
        }

        public async Task<OurGuild> CreateAsync(ulong guildId)
        {
            var guild = new OurGuild { Id = guildId };
            _dbContext.Guilds.Add(guild);
            await _dbContext.SaveChangesAsync();
            return guild;
        }

        public async Task<bool> SetQuoteChannelAsync(ulong guildId, ulong channelId)
        {
            var guild = await _dbContext.Guilds.FindAsync(guildId);
            if (guild == null) return false;
            guild.QuoteChanelId = channelId;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetModeratorRoleAsync(ulong guildId, ulong roleId)
        {
            var guild = await _dbContext.Guilds.FindAsync(guildId);
            if (guild == null) return false;
            guild.ModeratorRoleId = roleId;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
