﻿using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Logical.Enums;
using System.Collections.Generic;

namespace LeadershipMinion.Core.Configurations
{
    public class BasicConfiguration : IBasicConfiguration
    {
        public ulong StaffRole { get; set; }
        public ulong StaffChannel { get; set; }
        public ulong DebugChannel { get; set; }
        public List<ulong> ApplicationChannels { get; set; }
        public List<string> FunFacts { get; set; }
        public SystemEnvironment Environment { get; set; }
        public ulong CalBotId { get; set; }
    }
}