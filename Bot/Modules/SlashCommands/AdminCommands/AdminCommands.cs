using Application.Services;
using Bot.Services;
using Bot.Services.CommandServices;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Modules.SlashCommands.AdminCommands
{
    public partial class AdminCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private AdminService _adminService;

        public AdminCommands(AdminService adminService)
        {
            _adminService = adminService;
        }
    }
}
