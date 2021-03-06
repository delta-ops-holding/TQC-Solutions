using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models.V3
{
    public class ClanV3 : Guild
    {
        private Member _founder;

        public ClanV3(string name, string about, ClanPlatform clanPlatform, int identifier, Member founder) : base(name, about, clanPlatform, identifier)
        {
            _founder = founder;
        }

        public Member Founder { get => _founder; set => _founder = value; }        
    }
}