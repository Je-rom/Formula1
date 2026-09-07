using GitFormula_1.DTOs.Responses;
using GitFormula_1.Models;

namespace GitFormula_1.Utils
{
    public static class ProfileCardMapper
    {
        public static ProfileCardDto ToCardDto(GitHubProfile profile)
        {
            return new ProfileCardDto
            {
                Username = profile.Username,
                DisplayName = profile.DisplayName,
                AvatarUrl = profile.AvatarUrl,
                PrimaryLanguage = profile.PrimaryLanguage,

                Overall = profile.ScoreCard?.Overall ?? 0,
                Pace = profile.ScoreCard?.Pace ?? 0,
                Offense = profile.ScoreCard?.Offense ?? 0,
                Defense = profile.ScoreCard?.Defense ?? 0,
                Racecraft = profile.ScoreCard?.Racecraft ?? 0,
                Experience = profile.ScoreCard?.Experience ?? 0,
                Mentality = profile.ScoreCard?.Mentality ?? 0,

                Badges = profile.ProfileBadges
                    .Select(pb => pb.Badge.DisplayName)
                    .ToList(),

                Stats = new ProfileCardStatsDto
                {
                    CommitsLastYear = profile.RawStats?.CommitsLastYear ?? 0,
                    StarsEarned = profile.RawStats?.StarsEarned ?? 0,
                    MergedPullRequests = profile.RawStats?.MergedPullRequests ?? 0,
                    Followers = profile.RawStats?.Followers ?? 0,
                    CodeReviews = profile.RawStats?.CodeReviews ?? 0,
                    LifetimeContributions = profile.RawStats?.LifetimeContributions ?? 0
                },

                FetchedAt = profile.FetchedAt
            };
        }
    }
}