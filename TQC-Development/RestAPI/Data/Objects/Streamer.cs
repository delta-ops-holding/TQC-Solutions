using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    /// <summary>
    /// This class stores data properties for a streamer.
    /// 
    /// Inherits <see cref="BaseEntity"/>.
    /// </summary>
    public class Streamer : BaseEntity
    {
        private string _name;
        private string _streamURL;
        private string _streamerImageURL;

        /// <summary>
        /// The name of the streamer.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// The URL of the streamer.
        /// </summary>
        public string StreamURL { get { return _streamURL; } set { _streamURL = value; } }

        /// <summary>
        /// The Image URL of the streamer.
        /// </summary>
        public string StreamerImageURL { get { return _streamerImageURL; } set { _streamerImageURL = value; } }
    }
}
