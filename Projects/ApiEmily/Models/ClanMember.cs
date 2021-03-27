using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public class ClanMember : Member
    {
        #region Fields
        private bool _isFounder;

        #endregion

        #region Properties
        public bool IsFounder { get => _isFounder; set => _isFounder = value; }
        #endregion

        #region Constructors
        public ClanMember(bool isFounder, string userName, int identifier) : base (userName, identifier)
        {
            _isFounder = isFounder;
        }
        #endregion
    }
}