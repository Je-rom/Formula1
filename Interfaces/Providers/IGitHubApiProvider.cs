using GitFormula_1.Providers.GitHubModels;

namespace GitFormula_1.Interfaces.Providers
{
    public interface IGitHubApiProvider
    {
        Task<GitHubUserData?> GetUserProfileAsync(string username);
        Task<GitHubContributionData> GetContributionDataAsync(string username);
        Task<GitHubRepositoryStats> GetGitHubRepositoryStatsAsync(string username);
    }
}