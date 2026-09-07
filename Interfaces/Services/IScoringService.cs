using GitFormula_1.Models;

namespace GitFormula_1.Interfaces.Services
{
    public interface IScoringService
    {
        ScoreCard ComputeScore(RawGitHubStats stats, DateTime accountCreatedAt);
    }
}