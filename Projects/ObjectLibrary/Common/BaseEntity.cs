using ObjectLibrary.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public abstract class BaseEntity : IBaseEntity
    {
        private readonly int _id;

        public BaseEntity(int id)
        {
            _id = id;
        }

        public int Id => _id;
    }
}
