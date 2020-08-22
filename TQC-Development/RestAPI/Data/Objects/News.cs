using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    /// <summary>
    /// This class stores data properties for a news.
    /// 
    /// Inherits <see cref="BaseEntity"/>.
    /// </summary>
    public class News : BaseEntity
    {
        private string _version;
        private string _title;
        private string _content;
        private DateTime _registeredDate;
        private DateTime _updatedDate;

        /// <summary>
        /// The news version.
        /// </summary>
        public string Version { get { return _version; } set { _version = value; } }

        /// <summary>
        /// The news title.
        /// </summary>
        public string Title { get { return _title; } set { _title = value; } }

        /// <summary>
        /// Contains the content of the news.
        /// </summary>
        public string Content { get { return _content; } set { _content = value; } }

        /// <summary>
        /// The datetime where the news was created.
        /// </summary>
        public DateTime RegisteredDateTime { get { return _registeredDate; } set { _registeredDate = value; } }

        /// <summary>
        /// The datetime for when the news were last updated.
        /// </summary>
        public DateTime LastUpdatedDateTime { get { return _updatedDate; } set { _updatedDate = value; } }
    }
}
