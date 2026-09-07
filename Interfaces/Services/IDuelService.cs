using GitFormula_1.DTOs.Responses;

namespace GitFormula_1.Interfaces.Services
{
    public interface IDuelService
    {
        Task<DuelResultDto?> CreateDuelAsync(string usernameA, string usernameB);
    }
}