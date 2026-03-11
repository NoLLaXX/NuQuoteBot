using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedConfiguration
{
    public class Config
    {
        public string Token { get; set; }
        public ulong TestServerId { get; set; }
        public ulong OwnerId { get; set; }
        public int SecsToCheckDB { get; set; } = 10;

    }
}
