using DataBaseInfrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<QuoteRepository>();
            services.AddScoped<OurMemberRepository>();
            services.AddScoped<OurGuildRepository>();

            return services;
        }
    }
}
