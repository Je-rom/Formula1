using System.ComponentModel.DataAnnotations;

namespace GitFormula_1.Models
{
    public class GitHubProfile
    {
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? DisplayName { get; set; }

        public string? AvatarUrl { get; set; }

        [MaxLength(100)]
        public string? PrimaryLanguage { get; set; }

        public DateTime AccountCreatedAt { get; set; }

        public DateTime FetchedAt { get; set; }

        public DateTime CacheExpiresAt { get; set; }

        public RawGitHubStats? RawStats { get; set; }
        public ScoreCard? ScoreCard { get; set; }
        public ICollection<ProfileBadge> ProfileBadges { get; set; } = new List<ProfileBadge>();
    }
}