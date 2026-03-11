using Discord.Interactions;
using System;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.OwnerCommands
{
    public partial class OwnerCommands
    {
        [SlashCommand("reregistercommands", "Перерегистрировать все слеш-команды")]
        public async Task ReRegisterCommands()
        {
            await DeferAsync(ephemeral: true);

            try
            {
                await _ownerService.ReRegisterSlashCommandsAsync();
                await FollowupAsync("Слеш-команды перерегистрированы.", ephemeral: true);
            }
            catch (Exception ex)
            {
                await FollowupAsync($"Ошибка перерегистрации: {ex.Message}", ephemeral: true);
            }
        }
    }
}