using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public class Clan : BaseEntity
    {
        #region Fields
        private string _name;
        private string _about;
        private ClanPlatform _clanPlatform;
        private IList<Member> _members;

        #endregion

        #region Properties
        public string Name { get => _name; set => _name = value; }
        public string About { get => _about; set => _about = value; }
        public ClanPlatform ClanPlatform { get => _clanPlatform; set => _clanPlatform = value; }
        public IList<Member> Members { get => _members; set => _members = value; }
        #endregion

        #region Constructors
        public Clan(string name, string about, ClanPlatform clanPlatform, IList<Member> members, uint identifier) : base (identifier)
        {
            _name = name;
            _about = about;
            _clanPlatform = clanPlatform;
            _members = members;
        }
        #endregion
    }
}