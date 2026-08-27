namespace GitFormula_1.Models
{
    public class DuelRecord
    {
        public Guid Id { get; set; }

        public Guid ProfileAId { get; set; }
        public GitHubProfile ProfileA { get; set; } = null!;

        public Guid ProfileBId { get; set; }
        public GitHubProfile ProfileB { get; set; } = null!;

        public Guid? WinnerProfileId { get; set; }
        public GitHubProfile? WinnerProfile { get; set; }

        public int ProfileAScore { get; set; }
        public int ProfileBScore { get; set; }

        public DateTime CreatedAt { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(100)]
        public string ShareSlug { get; set; } = string.Empty;
    }
}