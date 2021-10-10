using System.Collections.Generic;

namespace LeadershipMinion.Core.Helpers
{
    public class RuntimeHelper<TRuntime>
    {
        public readonly Stack<TRuntime> Applications = new(100);
    }
}
