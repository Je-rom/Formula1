using System.ComponentModel.DataAnnotations;

namespace GitFormula_1.Models
{
    public class ScoreCard
    {
        public Guid Id { get; set; }

        public Guid ProfileId { get; set; }
        public GitHubProfile Profile { get; set; } = null!;

        public int Pace { get; set; }
        public int Offense { get; set; }
        public int Defense { get; set; }
        public int Racecraft { get; set; }
        public int Experience { get; set; }
        public int Mentality { get; set; }
        public int Overall { get; set; }

        public DateTime ComputedAt { get; set; }

        public int AlgorithmVersion { get; set; }
    }
}