using GitFormula_1.Models;
using Microsoft.EntityFrameworkCore;

namespace GitFormula_1.Data
{
    public class GitFormula1DbContext : DbContext
    {
        public GitFormula1DbContext(DbContextOptions<GitFormula1DbContext> options)
            : base(options)
        {
        }

        public DbSet<GitHubProfile> GitHubProfiles => Set<GitHubProfile>();
        public DbSet<RawGitHubStats> RawGitHubStats => Set<RawGitHubStats>();
        public DbSet<ScoreCard> ScoreCards => Set<ScoreCard>();
        public DbSet<Badge> Badges => Set<Badge>();
        public DbSet<ProfileBadge> ProfileBadges => Set<ProfileBadge>();
        public DbSet<DuelRecord> DuelRecords => Set<DuelRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProfileBadge>()
                .HasKey(pb => new { pb.ProfileId, pb.BadgeId });

            modelBuilder.Entity<GitHubProfile>()
                .HasIndex(p => p.Username)
                .IsUnique();

            modelBuilder.Entity<DuelRecord>()
                .HasOne(d => d.ProfileA)
                .WithMany()
                .HasForeignKey(d => d.ProfileAId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DuelRecord>()
                .HasOne(d => d.ProfileB)
                .WithMany()
                .HasForeignKey(d => d.ProfileBId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DuelRecord>()
                .HasOne(d => d.WinnerProfile)
                .WithMany()
                .HasForeignKey(d => d.WinnerProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}