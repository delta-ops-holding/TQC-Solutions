using System.Collections.Generic;

namespace LeadershipMinion.Core.Abstractions
{
    public interface IBasicConfiguration
    {
        List<ulong> ApplicationChannels { get; set; }
        ulong DebugChannel { get; set; }
        List<string> FunFacts { get; set; }
        ulong StaffChannel { get; set; }
        ulong StaffRole { get; set; }
    }
}
