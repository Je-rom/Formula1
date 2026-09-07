using GitFormula_1.Data;
using GitFormula_1.Interfaces.Repository;
using GitFormula_1.Models;
using Microsoft.EntityFrameworkCore;

namespace GitFormula_1.Repositories
{
    public class BadgeRepository : IBadgeRepository
    {
        private readonly GitFormula1DbContext _context;

        public BadgeRepository(GitFormula1DbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, Badge>> GetAllAsDictionaryAsync()
        {
            return await _context.Badges.ToDictionaryAsync(b => b.Code);
        }
    }
}