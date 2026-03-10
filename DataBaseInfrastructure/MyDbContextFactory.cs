using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataBaseInfrastructure
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();

            optionsBuilder.UseSqlite(@"Data Source=/home/nolax/Desktop/mydir/sqlite/NQB.db");

            return new MyDbContext(optionsBuilder.Options);
        }
    }
}
