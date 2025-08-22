using DataBaseInfrastructure.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataBaseInfrastructure.Models
{
    public class Quote
    {
        public ulong Id { get; set; }
        [Required]
        public string Text { get; set; } = null!;
        [Required]
        public bool IsAnon { get; set; }
        [Required]
        public DateTime Created { get; set; } = DateTime.Now;

        [Required]
        public StatusOptions Status { get; set; } = StatusOptions.Pending;

        public ulong? MsgId { get; set; }

        [Required]
        public ulong OurMemberId { get; set; }
        public OurMember OurMember { get; set; } = null!;

        [Required]
        public ulong OurGuildId { get; set; }
        public OurGuild OurGuild { get; set; } = null!;
    }
}
