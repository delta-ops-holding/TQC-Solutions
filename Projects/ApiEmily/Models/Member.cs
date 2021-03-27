using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public abstract class Member : BaseEntity
    {
        #region Fields
        private string _userName;

        #endregion

        #region Properties
        public string UserName { get => _userName; set => _userName = value; }
        #endregion

        #region Constructors
        protected Member(string userName, int identifier) : base (identifier)
        {
            _userName = userName;
        }
        #endregion
    }
}