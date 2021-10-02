using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Core.Helpers;
using System;
using System.Collections.Generic;

namespace LeadershipMinion.Core.Configurations
{
    public class BotConfiguration : IBotConfiguration
    {
        private readonly string _token;

        public BotConfiguration()
        {
            _token = Environment.GetEnvironmentVariable(ConstantHelper.TOKEN_ENVIRONMENT_VARIABLE_NAME);
        }

        public string CommandPrefix { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string Token { get { return _token; } }
        public ulong StaffRole { get; set; }
        public ulong StaffChannel { get; set; }
        public ulong DebugChannel { get; set; }
        public List<ulong> Channels { get; set; }
        public List<string> FunFacts { get; set; }
    }
}
