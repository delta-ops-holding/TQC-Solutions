using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public abstract class Guild : BaseEntity
    {
        private string _name;
        private string _about;
        private ClanPlatform _clanPlatform;

        public string Name { get => _name; set => _name = value; }
        public string About { get => _about; set => _about = value; }
        public ClanPlatform ClanPlatform { get => _clanPlatform; set => _clanPlatform = value; }

        public Guild(string name, string about, ClanPlatform clanPlatform, int identifier) : base(identifier)
        {
            _name = name;
            _about = about;
            _clanPlatform = clanPlatform;
        }
    }
}