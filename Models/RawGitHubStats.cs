using System.ComponentModel.DataAnnotations;


namespace GitFormula_1.Models
{
    public class RawGitHubStats
    {
        public Guid Id { get; set; }

        public Guid ProfileId { get; set; }
        public GitHubProfile Profile { get; set; } = null!;

        public int CommitsLastYear { get; set; }
        public int MergedPullRequests { get; set; }
        public int CodeReviews { get; set; }
        public int IssuesClosed { get; set; }
        public int CollabContributions { get; set; }
        public int LifetimeContributions { get; set; }
        public int LongestStreakDays { get; set; }
        public int CurrentStreakDays { get; set; }
        public int Followers { get; set; }
        public int StarsEarned { get; set; }
        public int TopRepoStars { get; set; }

        // JSONB columns — stored as string, serialized/deserialized in the service layer
        public string LanguageBreakdownJson { get; set; } = "{}";
        public string RawContributionCalendarJson { get; set; } = "{}";
    }
}