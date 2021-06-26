using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public class ClanFounder : Member
    {
        public ClanFounder(int id, string userName, AuthorityType authorityType) : base(id, userName, authorityType)
        {
        }
    }
}
