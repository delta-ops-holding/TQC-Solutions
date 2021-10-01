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
    }
}
