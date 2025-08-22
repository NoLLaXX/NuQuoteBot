using DataBaseInfrastructure.Models;
using DataBaseInfrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OurGuildService
    {
        private readonly OurGuildRepository _guildRepository;
        private readonly OurMemberRepository _memberRepository;

        private readonly DataIntegrityService _dataIntegrityService;

        public OurGuildService(OurGuildRepository guildRepository, OurMemberRepository memberRepository, DataIntegrityService dataIntegrityService)
        {
                _guildRepository = guildRepository;
                _memberRepository = memberRepository;
                _dataIntegrityService = dataIntegrityService;
        }

        public async Task<bool> SetQuoteChannelAsync(ulong guildId, ulong channelId)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);

            var res = await _guildRepository.SetQuoteChannelAsync(guildId, channelId);
            return res;
        }


        public async Task<bool> SetModeratorRoleAsync(ulong guildId, ulong roleId)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);

            var res = await _guildRepository.SetModeratorRoleAsync(guildId, roleId);
            return res;
        }

        public async Task<ulong?> GetModeratorRoleAsync(ulong guildId)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(guildId);

            var guild = await _guildRepository.FindAsync(guildId);
            if (guild == null)
                return null;

            return guild.ModeratorRoleId;
        }

        public async Task<ulong?> GetQuoteChannelAsync(ulong ourGuildId)
        {
            await _dataIntegrityService.EnsureGuildExistsAsync(ourGuildId);

            var guild = await _guildRepository.FindAsync(ourGuildId);
            if (guild == null)
                return null;

            return guild.QuoteChanelId;
        }
    }
}
