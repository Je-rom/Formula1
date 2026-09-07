using GitFormula_1.Models;

namespace GitFormula_1.Interfaces.Repository
{
    public interface IBadgeRepository
    {
        Task<Dictionary<string, Badge>> GetAllAsDictionaryAsync();
    }
}