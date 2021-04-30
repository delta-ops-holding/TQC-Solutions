using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public class Clan : Guild
    {
        private readonly IList<Member> _members;

        public Clan(string name, string about, ClanPlatform clanPlatform, IList<Member> members, int identifier) : base(identifier: identifier, name: name, about: about, clanPlatform: clanPlatform)
        {
            _members = members;
        }

        public IList<Member> Members
        {
            get { return _members; }
        }
    }
}