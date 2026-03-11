using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SharedConfiguration;

namespace DataBaseInfrastructure
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            var conf = ConfigurationFactory.BuildConfiguration();
            
            optionsBuilder.UseSqlite(conf.GetConnectionString("DefaultConnection"));

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
