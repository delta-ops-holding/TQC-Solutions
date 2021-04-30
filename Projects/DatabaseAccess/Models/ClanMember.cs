using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public class ClanMember : Member
    {
        private readonly bool _isFounder;

        public ClanMember(bool isFounder, string userName, int identifier) : base(userName, identifier)
        {
            _isFounder = isFounder;
        }

        public bool IsFounder
        {
            get { return _isFounder; }
        }
    }
}