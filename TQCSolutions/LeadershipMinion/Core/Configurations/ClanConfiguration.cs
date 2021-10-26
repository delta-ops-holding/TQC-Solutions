using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Models;
using System.Collections.Generic;

namespace LeadershipMinion.Core.Configurations
{
    public class ClanConfiguration : IClanConfiguration
    {
        public List<ClanDataModel> Clans { get; set; }
    }
}
