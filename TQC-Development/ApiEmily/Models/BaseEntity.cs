using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiEmily.Models
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// The Identifier for the entity.
        /// </summary>
        public int BaseId { get; set; }
    }
}