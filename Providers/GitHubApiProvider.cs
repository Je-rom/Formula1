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
            _httpClient = httpClientFactory.CreateClient("GitHubClient");
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
    }
}