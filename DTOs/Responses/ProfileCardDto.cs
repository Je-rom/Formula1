namespace GitFormula_1.DTOs.Responses
{
    public class ProfileCardDto
    {
        public string Username { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? PrimaryLanguage { get; set; }

        public int Overall { get; set; }
        public int Pace { get; set; }
        public int Offense { get; set; }
        public int Defense { get; set; }
        public int Racecraft { get; set; }
        public int Experience { get; set; }
        public int Mentality { get; set; }

        public List<string> Badges { get; set; } = new();

        public ProfileCardStatsDto Stats { get; set; } = new();

        public DateTime FetchedAt { get; set; }
    }

    public class ProfileCardStatsDto
    {
        public int CommitsLastYear { get; set; }
        public int StarsEarned { get; set; }
        public int MergedPullRequests { get; set; }
        public int Followers { get; set; }
        public int CodeReviews { get; set; }
        public int LifetimeContributions { get; set; }
    }
}