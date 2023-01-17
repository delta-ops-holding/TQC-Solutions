using LeadershipMinion.Logical.Enums;
using System.Collections.Generic;

namespace LeadershipMinion.Core.Abstractions
{
    public interface IBasicConfiguration
    {
        List<ulong> ApplicationChannels { get; }
        ulong DebugChannel { get; }
        List<string> FunFacts { get; }
        ulong StaffChannel { get; }
        ulong CalBotId { get; }
        ulong StaffRole { get; }
        public SystemEnvironment Environment { get; }
    }
}