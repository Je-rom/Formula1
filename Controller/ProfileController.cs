
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GitFormula_1.Controllers
{
    [ApiController]
    [Route("api/profiles")]

    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            var profile = await _profileService.GetOrFetchProfileAsync(username);

            if (profile == null)
            {
                return NotFound(new { message = $"GitHub user '{username}' not found." });
            }

            var dto = ProfileCardMapper.ToCardDto(profile);
            return Ok(dto);
        }
    }
}