using GitFormula_1.Models;

namespace GitFormula_1.Interfaces.Repository
{
    public interface IProfileRepository
    {
        Task<GitHubProfile?> GetByUsernameAsync(string username);
        Task<GitHubProfile> AddAsync(GitHubProfile profile);
        Task SaveChangesAsync();
    }
}