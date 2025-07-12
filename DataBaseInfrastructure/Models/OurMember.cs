using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure.Models
{
    public class OurMember
    {
        public ulong Id { get; set; }
        public virtual List<Quote> Quotes { get; set; } = new List<Quote>();
    }
}
    