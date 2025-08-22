using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure.Models
{
    public class OurGuild
    {
        public ulong Id { get; set; }
        public ulong? ModeratorRoleId { get; set; }
        public ulong? QuoteChanelId { get; set; }
    }
}
