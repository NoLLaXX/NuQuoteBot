using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseInfrastructure.Repository;

namespace Application.Services
{
    public class OurMemberService
    {
        private readonly OurMemberRepository _memberRepository;
        private readonly DataIntegrityService _dataIntegrityService;

        public OurMemberService(OurMemberRepository memberRepository, DataIntegrityService dataIntegrityService)
        {
            _memberRepository = memberRepository;
            _dataIntegrityService = dataIntegrityService;
        }
    }
}
