using System.ComponentModel.DataAnnotations;

namespace GitFormula_1.Models
{
    public class Badge
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;

        public int Priority { get; set; }

        public ICollection<ProfileBadge> ProfileBadges { get; set; } = new List<ProfileBadge>();
    }
}