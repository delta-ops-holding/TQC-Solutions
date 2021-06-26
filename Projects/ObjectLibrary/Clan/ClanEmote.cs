using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public class ClanEmote : BaseEntity
    {
        private readonly long _emoteId;

        public ClanEmote(int clanId, long emoteId) : base (clanId)
        {
            _emoteId = emoteId;
        }

        public long EmoteID => _emoteId;
    }
}
