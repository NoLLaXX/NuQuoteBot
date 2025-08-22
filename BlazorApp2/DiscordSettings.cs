namespace BlazorApp2
{
    public class DiscordSettings
    {
        public ulong? ClientId { get; set; }
        public string? ClientSecret { get; set; } = null!;
        public string? RedirectUrl { get; set; } = null!;
        public string? DiscordRedirectUrl { get; set; } = null!;
        public ulong? GuildId { get; set; } = null!;
    }
}
