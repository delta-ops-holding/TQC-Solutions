using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public class MentionRole : BaseEntity
    {
        private readonly long _mentionRoleId;

        public MentionRole(int clanId, long mentionRoleId) : base(clanId)
        {
            _mentionRoleId = mentionRoleId;
        }

        public long MentionRoleId => _mentionRoleId;
    }
}
