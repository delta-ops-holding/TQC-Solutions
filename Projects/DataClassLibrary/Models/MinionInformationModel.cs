using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClassLibrary.Models
{
    public class MinionInformationModel
    {
        public MinionInformationModel(Guid tokenId, string name, string version, string token, List<DisplayStatusModel> displayStatuses)
        {
            TokenId = tokenId;
            Name = name;
            Version = version;
            Token = token;
            DisplayStatuses = displayStatuses;
        }

        public Guid TokenId { get; }
        public string Name { get; }
        public string Version { get; }
        public string Token { get; }
        public List<DisplayStatusModel> DisplayStatuses { get; }
    }
}
