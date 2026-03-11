using Bot.Attributes;
using Bot.Services.CommandServices;
using Discord.Interactions;

namespace Bot.Modules.SlashCommands.ModerCommands
{
    [RequireModerRole]
    public partial class ModerCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly ModerService _moderService;

        public ModerCommands(ModerService moderService)
        {
            _moderService = moderService;
        }
    }
}
