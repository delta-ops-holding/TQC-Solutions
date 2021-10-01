using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadershipMinion.Core.Abstractions
{
    public interface IStartupService
    {
        Task InitializeBotAsync();
    }
}
