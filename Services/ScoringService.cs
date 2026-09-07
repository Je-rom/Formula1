using GitFormula_1.Constants;
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Models;
using GitFormula_1.Utils;

namespace GitFormula_1.Services
{
    public class ScoringService : IScoringService
    {
        public ScoreCard ComputeScore(RawGitHubStats stats, DateTime accountCreatedAt)
        {
            var pace = NormalizationHelpers.LogScale(
                ScoringConstants.PaceFloor, ScoringConstants.PaceMultiplier, stats.CommitsLastYear);

            var offense = NormalizationHelpers.LogScale(
                ScoringConstants.OffenseFloor, ScoringConstants.OffenseMultiplier, stats.MergedPullRequests);

            var defense = NormalizationHelpers.LogScale(
                ScoringConstants.DefenseFloor, ScoringConstants.DefenseMultiplier,
                stats.CodeReviews + stats.IssuesClosed);

            var racecraft = NormalizationHelpers.LogScale(
                ScoringConstants.RacecraftFloor, ScoringConstants.RacecraftMultiplier, stats.CollabContributions);

            var accountAgeYears = (DateTime.UtcNow - accountCreatedAt).TotalDays / 365.0;
            var experienceRaw = (accountAgeYears * ScoringConstants.ExperienceYearsMultiplier)
                + Math.Log2(stats.LifetimeContributions + 1) * ScoringConstants.ExperienceContributionsMultiplier;
            var experience = NormalizationHelpers.Clamp(experienceRaw);

            var mentalityRaw = ScoringConstants.MentalityFloor
                + (stats.LongestStreakDays * ScoringConstants.MentalityStreakMultiplier)
                + Math.Log2(stats.Followers + stats.StarsEarned + 1) * ScoringConstants.MentalitySocialMultiplier;
            var mentality = NormalizationHelpers.Clamp(mentalityRaw);

            var overall = NormalizationHelpers.Clamp(
                (pace * ScoringConstants.PaceWeight) +
                (offense * ScoringConstants.OffenseWeight) +
                (defense * ScoringConstants.DefenseWeight) +
                (racecraft * ScoringConstants.RacecraftWeight) +
                (experience * ScoringConstants.ExperienceWeight) +
                (mentality * ScoringConstants.MentalityWeight));

            return new ScoreCard
            {
                Id = Guid.NewGuid(),
                ProfileId = stats.ProfileId,
                Pace = pace,
                Offense = offense,
                Defense = defense,
                Racecraft = racecraft,
                Experience = experience,
                Mentality = mentality,
                Overall = overall,
                ComputedAt = DateTime.UtcNow,
                AlgorithmVersion = ScoringConstants.CurrentAlgorithmVersion
            };
        }
    }
}