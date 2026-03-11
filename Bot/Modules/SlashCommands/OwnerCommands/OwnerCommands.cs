using Bot.Attributes;
using Bot.Services.CommandServices;
using Discord.Interactions;

namespace Bot.Modules.SlashCommands.OwnerCommands
{
    [RequireBotOwner]
    public partial class OwnerCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private OwnerService _ownerService;

        public OwnerCommands(OwnerService ownerService)
        {
            _ownerService = ownerService;
        }
    }
}