using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClassLibrary.Models
{
    public class DisplayStatusModel
    {
        public DisplayStatusModel(int id, Guid minionGuid, string message)
        {
            Id = id;
            MinionGuid = minionGuid;
            Message = message;
        }

        public int Id { get; }
        public Guid MinionGuid { get; }
        public string Message { get; }
    }
}
