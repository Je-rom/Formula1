using GitFormula_1.Data;
using GitFormula_1.Interfaces.Repository;
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Models;
using GitFormula_1.Providers;
using Microsoft.EntityFrameworkCore;

namespace GitFormula_1.Services
{
    public class BadgeService : IBadgeService
    {

        private readonly IBadgeRepository _badgeService;

        public BadgeService(IBadgeRepository badgeService)
        {
            _badgeService = badgeService;
        }

        public async Task<List<Badge>> DetermineBadgesAsync(RawGitHubStats stats, ScoreCard scoreCard, DateTime accountCreatedAt)
        {
            var allBadges = await _badgeService.GetAllAsDictionaryAsync();
            var earnedCodes = new List<string>();

            if (scoreCard.Pace >= 85)
                earnedCodes.Add("RAPID_FIRE");

            if (scoreCard.Offense >= 80 && scoreCard.Pace >= 70)
                earnedCodes.Add("WORKHORSE");

            var languageCount = CountMeaningfulLanguages(stats.LanguageBreakdownJson);
            if (languageCount >= 5)
                earnedCodes.Add("POLYGLOT");

            if (HasDominantLanguage(stats.LanguageBreakdownJson, 80))
                earnedCodes.Add("SPECIALIST");

            if (scoreCard.Racecraft >= 80)
                earnedCodes.Add("TEAM_PLAYER");

            if (scoreCard.Racecraft <= 40 && scoreCard.Offense >= 60)
                earnedCodes.Add("LONE_WOLF");

            if (scoreCard.Experience >= 85)
                earnedCodes.Add("VETERAN");

            var accountAgeYears = (DateTime.UtcNow - accountCreatedAt).TotalDays / 365.0;
            if (accountAgeYears < 1)
                earnedCodes.Add("ROOKIE");

            if (stats.LifetimeContributions >= 5000)
                earnedCodes.Add("PROLIFIC");

            if (scoreCard.Mentality >= 85)
                earnedCodes.Add("CONSISTENT");

            if (stats.Followers >= 500 || stats.StarsEarned >= 1000)
                earnedCodes.Add("CROWD_FAVORITE");

            if ((stats.IssuesClosed + stats.CodeReviews) >= 100)
                earnedCodes.Add("MAINTAINER");


            var earnedBadges = earnedCodes
                .Where(code => allBadges.ContainsKey(code))
                .Select(code => allBadges[code])
                .OrderByDescending(b => b.Priority)
                .Take(3)
                .ToList();

            return earnedBadges;
        }

        private static int CountMeaningfulLanguages(string languageBreakdownJson)
        {
            var breakdown = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, double>>(languageBreakdownJson)
                ?? new Dictionary<string, double>();

            return breakdown.Count(kvp => kvp.Value >= 5.0);
        }

        private static bool HasDominantLanguage(string languageBreakdownJson, double thresholdPercent)
        {
            var breakdown = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, double>>(languageBreakdownJson)
                ?? new Dictionary<string, double>();

            return breakdown.Any(kvp => kvp.Value >= thresholdPercent);
        }

    }
}