namespace GitFormula_1.DTOs.Responses
{
    public class DuelResultDto
    {
        public ProfileCardDto ProfileA { get; set; } = null!;
        public ProfileCardDto ProfileB { get; set; } = null!;

        public int ProfileAStatsWon { get; set; }
        public int ProfileBStatsWon { get; set; }
        public int TiedStats { get; set; }

        public double ProfileADominancePercent { get; set; }
        public double ProfileBDominancePercent { get; set; }

        public string? WinnerUsername { get; set; }

        public List<StatComparisonDto> StatComparisons { get; set; } = new();
    }

    public class StatComparisonDto
    {
        public string StatName { get; set; } = string.Empty;
        public int ProfileAValue { get; set; }
        public int ProfileBValue { get; set; }
        public string? WinnerUsername { get; set; }
    }
}