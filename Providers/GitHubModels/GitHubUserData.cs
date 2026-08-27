namespace GitFormula_1.Providers.GitHubModels
{
    public class GitHubUserData
    {
        public string Login { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Followers { get; set; }
        public int PublicRepos { get; set; }
    }
}