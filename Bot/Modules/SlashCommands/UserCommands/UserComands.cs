using Bot.Services.CommandServices;
using Discord.Interactions;

namespace Bot.Modules.SlashCommands.UserCommands
{
    public partial class UserCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly UserService _userService;

        public UserCommands(UserService userService)
        {
            _userService = userService;
        }
    }
}
