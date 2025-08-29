using System;
using System.Linq;
using System.Threading.Tasks;
using Bot.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Attributes
{
    public class RequireModerRoleAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider RootServiceProvider)
        {

            if (context.User is IGuildUser gUser)
            {
                using (var serviceScope = RootServiceProvider.CreateScope())
                {
                    var LocalServiceProvider = serviceScope.ServiceProvider;

                    var discordUtility = LocalServiceProvider.GetRequiredService<DiscordUtilityService>();

                    var isModer = await discordUtility.IsUserModeratorAsync(gUser.Id, context.Guild.Id);

                    if (isModer) return PreconditionResult.FromSuccess();
                    else return PreconditionResult.FromError("Доступно только на модераторам");
                }
            }

            return PreconditionResult.FromError("Доступно только на сервере");
        }
    }
}
