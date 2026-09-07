using System.Net.Http.Headers;
using System.Text.Json;
using GitFormula_1.Interfaces.Providers;
using GitFormula_1.Providers.GitHubModels;

namespace GitFormula_1.Providers
{
    public class GitHubApiProvider : IGitHubApiProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _token;

        public GitHubApiProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient("GitHub");
            _token = configuration["GitHub:Token"]
                ?? throw new InvalidOperationException("GitHub token not configured");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token);
        }

        public async Task<GitHubUserData?> GetUserProfileAsync(string username)
        {
            var response = await _httpClient.GetAsync($"users/{username}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<JsonElement>(json);

            return new GitHubUserData
            {
                Login = doc.GetProperty("login").GetString() ?? username,
                Name = doc.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null,
                AvatarUrl = doc.TryGetProperty("avatar_url", out var avatarEl) ? avatarEl.GetString() : null,
                CreatedAt = doc.GetProperty("created_at").GetDateTime(),
                Followers = doc.GetProperty("followers").GetInt32(),
                PublicRepos = doc.GetProperty("public_repos").GetInt32()
            };
        }

        public async Task<GitHubContributionData> GetContributionDataAsync(string username)
        {
            const string query = @"
                query($username: String!) {
                    user(login: $username) {
                        contributionsCollection {
                            totalCommitContributions
                            totalPullRequestContributions
                            totalPullRequestReviewContributions
                            totalIssueContributions
                            contributionCalendar {
                                totalContributions
                                weeks {
                                    contributionDays {
                                        date
                                        contributionCount
                                    }
                                }
                            }
                        }
                    }
                }";

            var requestBody = new
            {
                query,
                variables = new { username }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/graphql")
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<JsonElement>(json);

            var collection = doc.GetProperty("data")
                                 .GetProperty("user")
                                 .GetProperty("contributionsCollection");

            var calendar = collection.GetProperty("contributionCalendar");
            var days = new List<DailyContribution>();

            foreach (var week in calendar.GetProperty("weeks").EnumerateArray())
            {
                foreach (var day in week.GetProperty("contributionDays").EnumerateArray())
                {
                    days.Add(new DailyContribution
                    {
                        Date = DateOnly.Parse(day.GetProperty("date").GetString()!),
                        Count = day.GetProperty("contributionCount").GetInt32()
                    });
                }
            }

            return new GitHubContributionData
            {
                TotalContributionsLastYear = calendar.GetProperty("totalContributions").GetInt32(),
                TotalCommitContributions = collection.GetProperty("totalCommitContributions").GetInt32(),
                TotalPullRequestContributions = collection.GetProperty("totalPullRequestContributions").GetInt32(),
                TotalPullRequestReviewContributions = collection.GetProperty("totalPullRequestReviewContributions").GetInt32(),
                TotalIssueContributions = collection.GetProperty("totalIssueContributions").GetInt32(),
                Calendar = days
            };
        }

        public async Task<GitHubRepositoryStats> GetGitHubRepositoryStatsAsync(string username)
        {
            const string query = @"
    query($username: String!) {
        user(login: $username) {
            pullRequests(states: MERGED) {
                totalCount
            }
            issues(states: CLOSED) {
                totalCount
            }
            issueComments {
                totalCount
            }
       repositories(first: 100, ownerAffiliations: OWNER, isFork: false, orderBy: {field: STARGAZERS, direction: DESC}) {
                nodes {
                    stargazerCount
                    languages(first: 10, orderBy: {field: SIZE, direction: DESC}) {
                        edges {
                            size
                            node {
                                name
                            }
                        }
                    }
                }
            }
        }
    }";
            var requestBody = new
            {
                query,
                variables = new { username }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/graphql")
            {
                Content = JsonContent.Create(requestBody)
            };

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonSerializer.Deserialize<JsonElement>(json);

            if (doc.TryGetProperty("errors", out var errors))
            {
                throw new InvalidOperationException($"GitHub GraphQL error: {errors}");
            }

            var user = doc.GetProperty("data").GetProperty("user");

            var mergedPrs = user.GetProperty("pullRequests").GetProperty("totalCount").GetInt32();
            var issuesClosed = user.GetProperty("issues").GetProperty("totalCount").GetInt32();
            var commentsMade = user.GetProperty("issueComments").GetProperty("totalCount").GetInt32();

            var repos = user.GetProperty("repositories").GetProperty("nodes");

            int starsEarned = 0;
            int topRepoStars = 0;
            var languageSizes = new Dictionary<string, long>();

            foreach (var repo in repos.EnumerateArray())
            {
                var stars = repo.GetProperty("stargazerCount").GetInt32();
                starsEarned += stars;
                if (stars > topRepoStars)
                    topRepoStars = stars;

                foreach (var edge in repo.GetProperty("languages").GetProperty("edges").EnumerateArray())
                {
                    var langName = edge.GetProperty("node").GetProperty("name").GetString()!;
                    var size = edge.GetProperty("size").GetInt64();

                    languageSizes[langName] = languageSizes.GetValueOrDefault(langName, 0) + size;
                }
            }

            long totalSize = languageSizes.Values.Sum();
            var languageBreakdown = totalSize > 0
                ? languageSizes.ToDictionary(
                    kvp => kvp.Key,
                    kvp => Math.Round((double)kvp.Value / totalSize * 100, 1))
                : new Dictionary<string, double>();

            return new GitHubRepositoryStats
            {
                MergedPullRequests = mergedPrs,
                IssuesClosed = issuesClosed,
                StarsEarned = starsEarned,
                TopRepoStars = topRepoStars,
                CommentsMade = commentsMade,
                LanguageBreakdown = languageBreakdown
            };
        }
    }
}