using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public class ClanFounder : Member
    {
        private readonly bool _isFounder;

        public ClanFounder(int identity, string userName, bool isFounder) : base(identifier: identity, userName: userName)
        {
            _isFounder = isFounder;
        }

        public bool IsFounder
        {
            get { return _isFounder; }
        }
    }
}