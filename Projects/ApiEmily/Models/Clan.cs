using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public class Clan : Guild
    {
        #region Fields
        private IList<Member> _members;

        #endregion

        #region Properties
        public IList<Member> Members { get => _members; set => _members = value; }
        #endregion

        #region Constructors
        public Clan(string name, string about, ClanPlatform clanPlatform, IList<Member> members, int identifier) : base (identifier: identifier, name: name, about: about, clanPlatform: clanPlatform)
        {
            _members = members;
        }
        #endregion
    }
}