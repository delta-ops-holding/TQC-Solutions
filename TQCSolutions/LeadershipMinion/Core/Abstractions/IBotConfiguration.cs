using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadershipMinion.Core.Abstractions
{
    public interface IBotConfiguration
    {
        string CommandPrefix { get; }
        string Version { get; }
        string Status { get; }
        string Token { get; }
        ulong StaffRole { get; }
        ulong StaffChannel { get; }
        ulong DebugChannel { get; }
        List<ulong> Channels { get; }
        List<string> FunFacts { get; }
    }
}