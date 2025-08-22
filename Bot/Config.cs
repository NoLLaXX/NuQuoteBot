using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class Config
    {
        public string Token { get; set; }
        public ulong TestServerId { get; set; }
        public int SecsToCheckDB { get; set; } = 10;
    }
}
