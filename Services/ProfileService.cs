using GitFormula_1.Interfaces.Providers;
using GitFormula_1.Interfaces.Repository;
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Models;

namespace GitFormula_1.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IGitHubApiProvider _gitHubApiProvider;
        private readonly IScoringService _scoringService;
        private readonly IBadgeService _badgeService;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

        public ProfileService(IProfileRepository profileRepository, IGitHubApiProvider gitHubApiProvider, IScoringService scoringService, IBadgeService badgeService)
        {
            _profileRepository = profileRepository;
            _gitHubApiProvider = gitHubApiProvider;
            _scoringService = scoringService;
            _badgeService = badgeService;
        }

        public async Task<GitHubProfile?> GetOrFetchProfileAsync(string username)
        {
            var normalizedUsername = username.ToLowerInvariant();
            var existing = await _profileRepository.GetByUsernameAsync(normalizedUsername);

            if (existing != null && existing.CacheExpiresAt > DateTime.UtcNow)
            {
                return existing;
            }

            var userData = await _gitHubApiProvider.GetUserProfileAsync(normalizedUsername);
            if (userData == null)
            {
                return null;
            }

            var contributionData = await _gitHubApiProvider.GetContributionDataAsync(normalizedUsername);
            var repoStats = await _gitHubApiProvider.GetGitHubRepositoryStatsAsync(normalizedUsername);

            var primaryLanguage = repoStats.LanguageBreakdown.Count > 0
                ? repoStats.LanguageBreakdown.OrderByDescending(kvp => kvp.Value).First().Key
                : null;

            if (existing == null)
            {
                var profile = new GitHubProfile
                {
                    Id = Guid.NewGuid(),
                    Username = normalizedUsername,
                    DisplayName = userData.Name,
                    AvatarUrl = userData.AvatarUrl,
                    PrimaryLanguage = primaryLanguage,
                    AccountCreatedAt = userData.CreatedAt,
                    FetchedAt = DateTime.UtcNow,
                    CacheExpiresAt = DateTime.UtcNow.Add(CacheDuration),
                    RawStats = BuildRawStats(userData, contributionData, repoStats)
                };
                profile.ScoreCard = _scoringService.ComputeScore(profile.RawStats, profile.AccountCreatedAt);

                var earnedBadges = await _badgeService.DetermineBadgesAsync(profile.RawStats, profile.ScoreCard, profile.AccountCreatedAt);

                profile.ProfileBadges.Clear();
                foreach (var badge in earnedBadges)
                {
                    profile.ProfileBadges.Add(new ProfileBadge
                    {
                        ProfileId = profile.Id,
                        BadgeId = badge.Id,
                        EarnedAt = DateTime.UtcNow
                    });
                }

                await _profileRepository.AddAsync(profile);
                await _profileRepository.SaveChangesAsync();
                return profile;
            }
            else
            {
                existing.DisplayName = userData.Name;
                existing.AvatarUrl = userData.AvatarUrl;
                existing.PrimaryLanguage = primaryLanguage;
                existing.FetchedAt = DateTime.UtcNow;
                existing.CacheExpiresAt = DateTime.UtcNow.Add(CacheDuration);

                if (existing.RawStats != null)
                {
                    UpdateRawStats(existing.RawStats, userData, contributionData, repoStats);
                }
                else
                {
                    existing.RawStats = BuildRawStats(userData, contributionData, repoStats);
                }
                existing.ScoreCard = _scoringService.ComputeScore(existing.RawStats, existing.AccountCreatedAt);

                await _profileRepository.SaveChangesAsync();
                return existing;
            }
        }

        private RawGitHubStats BuildRawStats(
            Providers.GitHubModels.GitHubUserData userData,
            Providers.GitHubModels.GitHubContributionData contributionData,
            Providers.GitHubModels.GitHubRepositoryStats repoStats)
        {
            var stats = new RawGitHubStats { Id = Guid.NewGuid() };
            UpdateRawStats(stats, userData, contributionData, repoStats);
            return stats;
        }

        private void UpdateRawStats(
            RawGitHubStats stats,
            Providers.GitHubModels.GitHubUserData userData,
            Providers.GitHubModels.GitHubContributionData contributionData,
            Providers.GitHubModels.GitHubRepositoryStats repoStats)
        {
            stats.CommitsLastYear = contributionData.TotalCommitContributions;
            stats.MergedPullRequests = repoStats.MergedPullRequests;
            stats.CodeReviews = contributionData.TotalPullRequestReviewContributions;
            stats.IssuesClosed = repoStats.IssuesClosed;
            stats.CollabContributions = repoStats.CommentsMade;
            stats.LifetimeContributions = contributionData.TotalContributionsLastYear;
            stats.Followers = userData.Followers;
            stats.StarsEarned = repoStats.StarsEarned;
            stats.TopRepoStars = repoStats.TopRepoStars;

            stats.LanguageBreakdownJson = System.Text.Json.JsonSerializer.Serialize(repoStats.LanguageBreakdown);
            stats.RawContributionCalendarJson = System.Text.Json.JsonSerializer.Serialize(contributionData.Calendar);

            var (longestStreak, currentStreak) = Utils.StreakCalculator.Calculate(contributionData.Calendar);
            stats.LongestStreakDays = longestStreak;
            stats.CurrentStreakDays = currentStreak;
        }
    }
}