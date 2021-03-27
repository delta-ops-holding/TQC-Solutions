using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Objects
{
    public class User : BaseEntity
    {
        private string _userName;
        private int _userTagId;
        private string _userSaltKey;
        private string _userPassword;
        private DateTime _userRegistrationDate;

        // Foreign Key Values:
        private int _adminForClanId; // Relation to the clan, the user is admin for.
        private int _pronounId; // The identifier of the pronoun for the user.
        private int _accessRoleId; // The identifier of the access role, the user is associated with.

        public string UserName { get => _userName; set => _userName = value; }
        public int UserTagId { get => _userTagId; set => _userTagId = value; }
        public string UserSaltKey { get => _userSaltKey; set => _userSaltKey = value; }
        public string UserPassword { get => _userPassword; set => _userPassword = value; }
        public DateTime UserRegistrationDate { get => _userRegistrationDate; set => _userRegistrationDate = value; }

        /// <summary>
        /// Relation to the clan, the user is admin for.
        /// </summary>
        public int AdminForClanId { get => _adminForClanId; set => _adminForClanId = value; }

        /// <summary>
        /// The identifier of the pronoun for the user.
        /// </summary>
        public int PronounId { get => _pronounId; set => _pronounId = value; }

        /// <summary>
        /// The identifier of the access role, the user is associated with.
        /// </summary>
        public int AccessRoleId { get => _accessRoleId; set => _accessRoleId = value; }
    }
}
