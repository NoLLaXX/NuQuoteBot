using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp2.Authrization
{
    public class GuildMemberHandler : AuthorizationHandler<GuildMemberRequirement>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DiscordSettings _discordSettings;

        public GuildMemberHandler(IHttpClientFactory httpClientFactory, DiscordSettings discordSettings)
        {
            _httpClientFactory = httpClientFactory;
            _discordSettings = discordSettings;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GuildMemberRequirement requirement)
        {
            var accessToken = context.User.FindFirst("access_token")?.Value;
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var guildId = _discordSettings.GuildId?.ToString();

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(guildId))
                return;

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync("https://discord.com/api/users/@me/guilds");
            if (!response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();
            var guilds = JsonDocument.Parse(content).RootElement;
            bool isMember = guilds.EnumerateArray().Any(g => g.GetProperty("id").GetString() == guildId);
            if (isMember)
                context.Succeed(requirement);
        }
    }
}
