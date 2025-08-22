using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInfrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseInfrastructure.Repository
{
    public class OurMemberRepository(MyDbContext dbContext)
    {
        public async Task<OurMember?> FindAsync(ulong userId)
        {
            return await dbContext.OurMembers.FindAsync(userId);
        }

        public async Task<OurMember> AddAsync(ulong userId)
        {
            var member = new OurMember { Id = userId };
            dbContext.OurMembers.Add(member);
            await dbContext.SaveChangesAsync();
            return member;
        }
    }
}
