using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmily.Models
{
    /// <summary>
    /// This class stores data properties for about us.
    /// 
    /// Inherits <see cref="BaseEntity"/>.
    /// </summary>
    public class AboutUs : BaseEntity
    {
        private string _welcome;
        private string _mission;
        private string _vision;
        private DateTime _createdDate;
        private DateTime _updatedDate;

        /// <summary>
        /// The welcome message for the about us.
        /// </summary>
        public string WelcomeMessage { get { return _welcome; } set { _welcome = value; } }

        /// <summary>
        /// The mission for the community.
        /// </summary>
        public string Mission { get { return _mission; } set { _mission = value; } }

        /// <summary>
        /// The vision for the community.
        /// </summary>
        public string Vision { get { return _vision; } set { _vision = value; } }

        /// <summary>
        /// The datetime of when the about us was created.
        /// </summary>
        public DateTime RegisteredDateTime { get { return _createdDate; } set { _createdDate = value; } }

        /// <summary>
        /// The datetime of when the about us was last updated.
        /// </summary>
        public DateTime LastUpdatedDateTime { get { return _updatedDate; } set { _updatedDate = value; } }
    }
}
