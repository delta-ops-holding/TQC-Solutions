using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Models.V3
{
    public class ClanAdminV3 : Member
    {
        public ClanAdminV3(string userName, int identifier) : base(userName, identifier)
        {
        }
    }
}
