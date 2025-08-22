using DataBaseInfrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DataIntegrityService
    {
        private readonly OurMemberRepository _memberRepository;
        private readonly OurGuildRepository _guildRepository;
        private readonly IMemoryCache _cache;

        public DataIntegrityService(OurMemberRepository memberRepository, OurGuildRepository guildRepository, IMemoryCache cache)
        {
            _memberRepository = memberRepository;
            _guildRepository = guildRepository;
            _cache = cache;
        }

        public async Task EnsureMemberExistsAsync(ulong userId)
        {
            if (!_cache.TryGetValue($"member_{userId}", out _))
            {
                var member = await _memberRepository.FindAsync(userId);
                if (member == null)
                    await _memberRepository.AddAsync(userId);
                _cache.Set($"member_{userId}", true, TimeSpan.FromMinutes(10));
            }
        }

        public async Task EnsureGuildExistsAsync(ulong guildId)
        {
            if (!_cache.TryGetValue($"guild_{guildId}", out _))
            {
                var guild = await _guildRepository.FindAsync(guildId);
                if (guild == null)
                    await _guildRepository.CreateAsync(guildId);
                _cache.Set($"guild_{guildId}", true, TimeSpan.FromMinutes(10));
            }
        }
    }
}
