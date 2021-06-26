using ObjectLibrary.Clan.Abstractions;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public abstract class Member : BaseEntity, IMember
    {
        private readonly string _userName;
        private readonly AuthorityType _authorityType;

        public Member(int id, string userName, AuthorityType authorityType) : base(id)
        {
            _userName = userName;
            _authorityType = authorityType;
        }

        public string UserName => _userName;

        public AuthorityType AuthorityType => _authorityType;
    }
}
