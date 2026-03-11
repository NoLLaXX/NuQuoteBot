using System;
using System.Threading.Tasks;
using Bot.Services;
using Discord;
using Discord.Interactions;

namespace Bot.Attributes
{
    public class RequireModerRoleAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo command, IServiceProvider rootServiceProvider)
        {
            if (context.Guild is null)
            {
                return PreconditionResult.FromError("Доступно только на сервере");
            }

            if (context.User is not IGuildUser gUser)
            {
                return PreconditionResult.FromError("Доступно только на сервере");
            }

            using var serviceScope = rootServiceProvider.CreateScope();
            var localServiceProvider = serviceScope.ServiceProvider;

            var discordUtility = localServiceProvider.GetRequiredService<DiscordUtilityService>();

            var isModer = await discordUtility.IsUserModeratorAsync(context.Guild.Id, gUser.Id);

            return isModer
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("Доступно только модераторам");
        }
    }
}
