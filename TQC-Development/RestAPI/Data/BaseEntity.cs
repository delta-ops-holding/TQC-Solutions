using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data
{
    public abstract class BaseEntity
    {
        private int _baseId;

        /// <summary>
        /// The Identifier for the entity.
        /// </summary>
        public int BaseId { get { return _baseId; } set { _baseId = value; } }
    }
}
