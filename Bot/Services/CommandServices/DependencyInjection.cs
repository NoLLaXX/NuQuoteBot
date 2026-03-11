using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Services.CommandServices
{
    public static class DependencyInjection
    {
        //TODO пока пусть существует в этом проекте, но надо перенести в Application, чтобы переиспользовать эту логику на сайте
        public static IServiceCollection AddCommandServices(this IServiceCollection services)
        {
            services.AddScoped<AdminService>();
            services.AddScoped<ModerService>();
            services.AddScoped<UserService>();
            services.AddScoped<OwnerService>();
            return services;
        }
    }
}
