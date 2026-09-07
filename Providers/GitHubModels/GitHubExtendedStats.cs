namespace GitFormula_1.Providers.GitHubModels
{
    public class GitHubRepositoryStats
    {
        public int MergedPullRequests { get; set; }
        public int IssuesClosed { get; set; }
        public int StarsEarned { get; set; }
        public int TopRepoStars { get; set; }
        public int CommentsMade { get; set; }
        public Dictionary<string, double> LanguageBreakdown { get; set; } = new();
    }
}