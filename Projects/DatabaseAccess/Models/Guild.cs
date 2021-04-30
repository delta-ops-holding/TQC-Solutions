using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public abstract class Guild : BaseEntity
    {
        private readonly string _name;
        private readonly string _about;
        private readonly ClanPlatform _clanPlatform;

        internal Guild(string name, string about, ClanPlatform clanPlatform, int identifier) : base(identifier)
        {
            _name = name;
            _about = about;
            _clanPlatform = clanPlatform;
        }

        public string Name
        {
            get { return _name; }
        }

        public string About
        {
            get { return _about; }
        }

        public ClanPlatform ClanPlatform
        {
            get { return _clanPlatform; }
        }
    }
}