namespace LeadershipMinion.Core.Helpers
{
    public static class ConstantHelper
    {
        public const string AUTHENTICATION_TOKEN = "TOKEN";
        public const string BOT_CONFIGURATION_SECTION = "Bot";
        public const string BASIC_CONFIGURATION_SECTION = "Basic";
        public const string CLAN_CONFIGURATION_SECTION = "Clan";

        /// <summary>
        /// Represents an integer that defines the cooldown in seconds.
        /// </summary>
        public const int GAME_ACTIVITY_COOLDOWN = 30;

        /// <summary>
        /// Represents an integer that defines the cooldown in hours.
        /// </summary>
        public const int APPLICATION_COOLDOWN = 24;

        /// <summary>
        /// Represents an integer that defines the interval in hours.
        /// </summary>
        public const int CLEAN_APPLICATIONS_INTERVAL = 8;

        /// <summary>
        /// Represents an integer that defines the capacity of applications.
        /// </summary>
        public const int APPLICATION_DICTIONARY_CAPACITY = 100;
    }
}