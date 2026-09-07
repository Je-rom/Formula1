using GitFormula_1.Data;
using GitFormula_1.Interfaces.Repository;
using GitFormula_1.Models;
using Microsoft.EntityFrameworkCore;

namespace GitFormula_1.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly GitFormula1DbContext _context;

        public ProfileRepository(GitFormula1DbContext context)
        {
            _context = context;
        }

        public async Task<GitHubProfile?> GetByUsernameAsync(string username)
        {
            return await _context.GitHubProfiles
                .Include(p => p.RawStats)
                .Include(p => p.ScoreCard)
                .Include(p => p.ProfileBadges)
                    .ThenInclude(pb => pb.Badge)
                .FirstOrDefaultAsync(p => p.Username == username);
        }

        public async Task<GitHubProfile> AddAsync(GitHubProfile profile)
        {
            await _context.GitHubProfiles.AddAsync(profile);
            return profile;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}