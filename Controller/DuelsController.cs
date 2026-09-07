using GitFormula_1.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitFormula_1.Controllers
{
    [ApiController]
    [Route("api/duels")]
    public class DuelsController : ControllerBase
    {
        private readonly IDuelService _duelService;

        public DuelsController(IDuelService duelService)
        {
            _duelService = duelService;
        }

        [HttpGet("{usernameA}/vs/{usernameB}")]
        public async Task<IActionResult> GetDuel(string usernameA, string usernameB)
        {
            var result = await _duelService.CreateDuelAsync(usernameA, usernameB);

            if (result == null)
            {
                return NotFound(new { message = "One or both GitHub users could not be found." });
            }

            return Ok(result);
        }
    }
}