namespace GitFormula_1.Providers.GitHubModels
{
    public class GitHubContributionData
    {
        public int TotalContributionsLastYear { get; set; }
        public int TotalCommitContributions { get; set; }
        public int TotalPullRequestContributions { get; set; }
        public int TotalPullRequestReviewContributions { get; set; }
        public int TotalIssueContributions { get; set; }
        public List<DailyContribution> Calendar { get; set; } = new();
    }

    public class DailyContribution
    {
        public DateOnly Date { get; set; }
        public int Count { get; set; }
    }
}
