using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    public class Pronoun : BaseEntity
    {
        private string pronounName;

        public string PronounName { get => pronounName; set => pronounName = value; }
    }
}
