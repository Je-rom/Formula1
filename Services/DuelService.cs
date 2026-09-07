using GitFormula_1.DTOs.Responses;
using GitFormula_1.Interfaces.Services;
using GitFormula_1.Utils;

namespace GitFormula_1.Services
{
    public class DuelService : IDuelService
    {
        private readonly IProfileService _profileService;

        public DuelService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<DuelResultDto?> CreateDuelAsync(string usernameA, string usernameB)
        {
            var profileA = await _profileService.GetOrFetchProfileAsync(usernameA);
            var profileB = await _profileService.GetOrFetchProfileAsync(usernameB);

            if (profileA == null || profileB == null)
            {
                return null;
            }

            var dtoA = ProfileCardMapper.ToCardDto(profileA);
            var dtoB = ProfileCardMapper.ToCardDto(profileB);

            var statComparisons = new List<StatComparisonDto>
            {
                CompareStat("Pace", dtoA.Pace, dtoB.Pace, dtoA.Username, dtoB.Username),
                CompareStat("Offense", dtoA.Offense, dtoB.Offense, dtoA.Username, dtoB.Username),
                CompareStat("Defense", dtoA.Defense, dtoB.Defense, dtoA.Username, dtoB.Username),
                CompareStat("Racecraft", dtoA.Racecraft, dtoB.Racecraft, dtoA.Username, dtoB.Username),
                CompareStat("Experience", dtoA.Experience, dtoB.Experience, dtoA.Username, dtoB.Username),
                CompareStat("Mentality", dtoA.Mentality, dtoB.Mentality, dtoA.Username, dtoB.Username),
            };

            var aWins = statComparisons.Count(s => s.WinnerUsername == dtoA.Username);
            var bWins = statComparisons.Count(s => s.WinnerUsername == dtoB.Username);
            var tied = statComparisons.Count(s => s.WinnerUsername == null);

            var totalA = dtoA.Pace + dtoA.Offense + dtoA.Defense + dtoA.Racecraft + dtoA.Experience + dtoA.Mentality;
            var totalB = dtoB.Pace + dtoB.Offense + dtoB.Defense + dtoB.Racecraft + dtoB.Experience + dtoB.Mentality;
            var combinedTotal = totalA + totalB;

            var dominanceA = combinedTotal > 0 ? Math.Round((double)totalA / combinedTotal * 100, 1) : 50.0;
            var dominanceB = combinedTotal > 0 ? Math.Round((double)totalB / combinedTotal * 100, 1) : 50.0;

            string? overallWinner = null;
            if (aWins > bWins) overallWinner = dtoA.Username;
            else if (bWins > aWins) overallWinner = dtoB.Username;

            return new DuelResultDto
            {
                ProfileA = dtoA,
                ProfileB = dtoB,
                ProfileAStatsWon = aWins,
                ProfileBStatsWon = bWins,
                TiedStats = tied,
                ProfileADominancePercent = dominanceA,
                ProfileBDominancePercent = dominanceB,
                WinnerUsername = overallWinner,
                StatComparisons = statComparisons
            };
        }

        private static StatComparisonDto CompareStat(string statName, int valueA, int valueB, string usernameA, string usernameB)
        {
            string? winner = valueA > valueB ? usernameA
                            : valueB > valueA ? usernameB
                            : null;

            return new StatComparisonDto
            {
                StatName = statName,
                ProfileAValue = valueA,
                ProfileBValue = valueB,
                WinnerUsername = winner
            };
        }
    }
}