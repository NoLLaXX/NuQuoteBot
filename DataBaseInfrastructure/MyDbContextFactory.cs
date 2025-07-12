using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataBaseInfrastructure
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            optionsBuilder.UseSqlite(@"Data Source=D:\MyDir\repos\.нужное\NuQuoteBot\NuQuoteBot\DataBaseInfrastructure\DB_Storage\appDB.db");

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
