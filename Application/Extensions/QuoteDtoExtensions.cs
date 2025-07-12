using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO;
using DataBaseInfrastructure.Models;

namespace Application.Extensions
{
    public static class QuoteDtoExtensions
    {
        public static QuoteDto ToDto(this Quote quote)
        {
            return new QuoteDto
            {
                Id = quote.Id,
                Text = quote.Text,
                IsAnon = quote.IsAnon,
                Status = quote.Status,
                MsgId = quote.MsgId,

                Created = quote.Created,

                OurMemberId = quote.OurMemberId,
                OurGuildId = quote.OurGuildId
            };
        }
    }
}
