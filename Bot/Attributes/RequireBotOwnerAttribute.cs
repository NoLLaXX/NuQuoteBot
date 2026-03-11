using Discord;
using Discord.Interactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SharedConfiguration;
using System;
using System.Threading.Tasks;

namespace Bot.Attributes
{
    public class RequireBotOwnerAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo command, IServiceProvider services)
        {
            var config = services.GetRequiredService<IOptions<Config>>().Value;

            if (context.User.Id == config.OwnerId)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError("Команда доступна только владельцу бота"));
        }
    }
}