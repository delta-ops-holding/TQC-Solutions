using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public abstract class Member : BaseEntity
    {
        private readonly string _userName;

        protected Member(string userName, int identifier) : base(identifier)
        {
            _userName = userName;
        }

        public string UserName
        {
            get { return _userName; }
        }        
    }
}