using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Core.Helpers;
using System;

namespace LeadershipMinion.Core.Configurations
{
    public class BotConfiguration : IBotConfiguration
    {
        private readonly string _token;

        public BotConfiguration()
        {
            _token = Environment.GetEnvironmentVariable(ConstantHelper.AUTHENTICATION_TOKEN);
        }

        public string CommandPrefix { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public string Token { get { return _token; } }
    }
}