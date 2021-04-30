using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models.V3
{
    public class ClanV3 : Guild
    {
        private readonly IList<Member> _admins;
        private readonly Member _founder;

        public ClanV3(string name, string about, ClanPlatform clanPlatform, Member founder, IList<Member> admins, int identifier) : base(name, about, clanPlatform, identifier)
        {
            _founder = founder;
            _admins = admins;
        }

        public Member Founder
        {
            get
            {
                return _founder;
            }
        }

        public IList<Member> Admins
        {
            get
            {
                return _admins;
            }
        }
    }
}