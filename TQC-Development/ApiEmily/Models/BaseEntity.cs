using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public abstract class BaseEntity
    {
        #region Fields
        private uint _identifier;
        #endregion

        #region Properties
        public uint Identifier { get => _identifier; set => _identifier = value; }
        #endregion

        #region Constructors
        protected BaseEntity(uint identifier)
        {
            _identifier = identifier;
        } 
        #endregion
    }
}