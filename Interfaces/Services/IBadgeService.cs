using GitFormula_1.Models;

namespace GitFormula_1.Interfaces.Services
{
    public interface IBadgeService
    {
        Task<List<Badge>> DetermineBadgesAsync(RawGitHubStats stats, ScoreCard scoreCard, DateTime accountCreatedAt);
    }
}