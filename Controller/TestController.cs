using GitFormula_1.Interfaces.Providers;
using Microsoft.AspNetCore.Mvc;

namespace GitFormula_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IGitHubApiProvider _gitHubApiProvider;

        public TestController(IGitHubApiProvider gitHubApiProvider)
        {
            _gitHubApiProvider = gitHubApiProvider;
        }

        [HttpGet("profile/{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var profile = await _gitHubApiProvider.GetUserProfileAsync(username);
            return Ok(profile);
        }

        [HttpGet("contributions/{username}")]
        public async Task<IActionResult> GetContributions(string username)
        {
            var data = await _gitHubApiProvider.GetContributionDataAsync(username);
            return Ok(data);
        }

        [HttpGet("repostats/{username}")]
        public async Task<IActionResult> GetExtendedStats(string username)
        {
            var stats = await _gitHubApiProvider.GetGitHubRepositoryStatsAsync(username);
            return Ok(stats);
        }
    }
}