using ObjectLibrary.Clan.Interfaces;
using ObjectLibrary.Core;

namespace ObjectLibrary.Clan
{
    public class Clan : BaseEntity, IClan
    {
        private readonly string _name;
        private readonly string _about;
        private readonly IPlatform _platform;
        private readonly IClanEmote _clanEmote;
        private readonly IMentionRole _mentionRole;

        public Clan(int id, string name, string about, IPlatform platform) : base(id)
        {
            _name = name;
            _about = about;
            _platform = platform;
        }

        public string Name { get { return _name; } }
        public string About { get { return _about; } }
        public IPlatform Platform { get { return _platform; } }
        public IClanEmote ClanEmote { get { return _clanEmote; } }
        public IMentionRole MentionRole { get { return _mentionRole; } }
    }
}
