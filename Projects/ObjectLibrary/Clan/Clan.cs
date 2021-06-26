using ObjectLibrary.Clan.Abstractions;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public class Clan : BaseEntity, IClan
    {
        private readonly string _name;
        private readonly string _about;
        private readonly Platform _platform;
        private ClanFounder _founder;
        private readonly List<ClanAdmin> _admins = new();

        public Clan(int id, string name, string about, Platform platform) : base(id)
        {
            _name = name;
            _about = about;
            _platform = platform;
        }

        public string Name => _name;

        public string About => _about;

        public ClanFounder Founder => _founder;

        public List<ClanAdmin> Admins => _admins;

        public Platform Platform => _platform;

        public void AddClanMember(ClanAdmin member)
        {
            _admins.Add(member);
        }

        public void AddClanFounder(ClanFounder founder)
        {
            _founder = founder;
        }
    }
}
