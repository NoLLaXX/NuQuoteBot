using Application.Services;
using DataBaseInfrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
        {
            services.AddDataServices(connectionString);

            services.AddMemoryCache();
            services.AddScoped<DataIntegrityService>();

            services.AddScoped<QuoteService>();
            services.AddScoped<OurGuildService>();
            services.AddScoped<OurMemberService>();

            return services;
        }
    }
}
