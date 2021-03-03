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
        private int _identifier;
        #endregion

        #region Properties
        public int Identifier { get => _identifier; set => _identifier = value; }
        #endregion

        #region Constructors
        protected BaseEntity(int identifier)
        {
            _identifier = identifier;
        } 
        #endregion
    }
}