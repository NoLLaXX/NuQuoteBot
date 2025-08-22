using DataBaseInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class QuoteDto
    {
        public ulong Id { get; set; }
        public string Text { get; set; } = null!;
        public bool IsAnon { get; set; }
        public DateTime Created { get; set; }


        public StatusOptions Status { get; set; }
        public ulong? MsgId { get; set; }

        public ulong OurMemberId { get; set; }
        public ulong OurGuildId { get; set; }
    }
}