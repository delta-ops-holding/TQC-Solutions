using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Models
{
    /// <summary>
    /// Represents a model for clan data.
    /// </summary>
    public class ClanDataModel
    {
        public int DeltaId { get; set; }
        public ulong EmoteId { get; set; }
        public ulong MentionRoleId { get; set; }
        public Clan Name { get; set; }
        public ClanPlatform Platform { get; set; }
        public string Icon { get; set; }
        public bool Disabled { get; set; }
    }
}
