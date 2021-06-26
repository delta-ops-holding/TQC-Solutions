using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public class ApplicationChannel : BaseEntity
    {
        private readonly long _channelId;

        public ApplicationChannel(int platformId, long channelId) : base(platformId)
        {
            _channelId = channelId;
        }

        public long ChannelId => _channelId;
    }
}
