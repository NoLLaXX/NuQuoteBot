using DataBaseInfrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public DbSet<OurMember> OurMembers { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<OurGuild> Guilds { get; set; }
    }
}
