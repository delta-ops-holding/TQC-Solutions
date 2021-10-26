using LeadershipMinion.Logical.Models;
using System.Collections.Generic;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface IClanConfiguration
    {
        List<ClanDataModel> Clans { get; }
    }
}
