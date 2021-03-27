using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    public class Clan : BaseEntity
    {
        private string _name;
        private string _about;
        private ClanPlatform _clanPlatform;
        private IList<ClanAuthority> _clanAuthorities;

        public string Name { get => _name; set => _name = value; }
        public string About { get => _about; set => _about = value; }
        public ClanPlatform ClanPlatform { get => _clanPlatform; set => _clanPlatform = value; }
        public IList<ClanAuthority> ClanAuthorities { get => _clanAuthorities; set => _clanAuthorities = value; }
    }
}
