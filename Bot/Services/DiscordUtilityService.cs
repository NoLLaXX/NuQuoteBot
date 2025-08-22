using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Services
{
    public class DiscordUtilityService
    {
        private readonly OurGuildService _guildService;
        private readonly DiscordSocketClient _discord;

        public DiscordUtilityService(OurGuildService guildService, DiscordSocketClient discord)
        {
            _guildService = guildService;
            _discord = discord;
        }

        public async Task<bool> IsUserModeratorAsync(ulong guildId, ulong userId)
        {
            var moderRoleId = await _guildService.GetModeratorRoleAsync(guildId);
            if (moderRoleId == null)
                return false;

            var guild = _discord.GetGuild(guildId);
            if (guild == null)
                return false;

            var user = guild.GetUser(userId) as IGuildUser;
            if (user == null)
                return false;

            return user.RoleIds.Contains(moderRoleId.Value);
        }

        public string GetGuildUserNick(ulong guildId, ulong userId)
        {
            var guild = _discord.GetGuild(guildId);
            if (guild == null) throw new ArgumentException("Guild not found");

            var user = guild.GetUser(userId) as IGuildUser;
            if (user == null) throw new ArgumentException("User not found in the guild");

            return user.Nickname ?? user.Username;
        }
    }


}
