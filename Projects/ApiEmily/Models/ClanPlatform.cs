using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public class ClanPlatform : BaseEntity
    {
        #region Fields
        private string _name;
        private string _platformImageURL;
        #endregion

        #region Properties
        public string Name { get => _name; set => _name = value; }
        public string PlatformImageURL { get => _platformImageURL; set => _platformImageURL = value; } 
        #endregion

        #region Constructors
        public ClanPlatform(string name, string platformImageURL, int identifier) : base(identifier)
        {
            _name = name;
            _platformImageURL = platformImageURL;
        }
        #endregion
    }
}