namespace GitFormula_1.Models
{
    public class ProfileBadge
    {
        public Guid ProfileId { get; set; }
        public GitHubProfile Profile { get; set; } = null!;

        public Guid BadgeId { get; set; }
        public Badge Badge { get; set; } = null!;

        public DateTime EarnedAt { get; set; }
    }
}