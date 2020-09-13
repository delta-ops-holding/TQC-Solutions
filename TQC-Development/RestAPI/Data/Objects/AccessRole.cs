using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    public class AccessRole : BaseEntity
    {
        private string roleName;
        private string roleDescription;

        public string RoleName { get => roleName; set => roleName = value; }
        public string RoleDescription { get => roleDescription; set => roleDescription = value; }
    }
}
