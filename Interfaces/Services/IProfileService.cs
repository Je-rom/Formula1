using GitFormula_1.Models;

namespace GitFormula_1.Interfaces.Services
{
    public interface IProfileService
    {
        Task<GitHubProfile?> GetOrFetchProfileAsync(string username);
    }
}