using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatabaseAccess.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class BaseEntity
    {
        private readonly int _identifier;

        internal BaseEntity(int identifier)
        {
            _identifier = identifier;
        }

        public int Identifier
        {
            get { return _identifier; }
        }
    }
}