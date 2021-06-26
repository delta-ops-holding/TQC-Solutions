using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public class AuthorityType : BaseEntity
    {
        private readonly string _typeName;

        public AuthorityType(int id, string typeName) : base(id)
        {
            _typeName = typeName;
        }

        public string TypeName => _typeName;
    }
}
