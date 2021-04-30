using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    public class ClanPlatform : BaseEntity
    {
        private readonly string _name;
        private readonly string _platformImageURL;

        public ClanPlatform(string name, string platformImageURL, int identifier) : base(identifier)
        {
            _name = name;
            _platformImageURL = platformImageURL;
        }

        public string Name
        {
            get { return _name; }
        }

        public string PlatformImageURL
        {
            get { return _platformImageURL; }
        }
    }
}