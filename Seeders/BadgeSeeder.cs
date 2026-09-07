using GitFormula_1.Data;
using GitFormula_1.Models;
using Microsoft.EntityFrameworkCore;

namespace GitFormula_1.Seeder
{
    public static class BadgeSeeder
    {
        public static async Task SeedAsync(GitFormula1DbContext context)
        {
            if (await context.Badges.AnyAsync())
            {
                return; //already seeded, skip
            }

            var badges = new List<Badge>
            {
                new() { Id = Guid.NewGuid(), Code = "RAPID_FIRE", DisplayName = "Rapid Fire", Priority = 5 },
                new() { Id = Guid.NewGuid(), Code = "WORKHORSE", DisplayName = "Workhorse", Priority = 5 },
                new() { Id = Guid.NewGuid(), Code = "POLYGLOT", DisplayName = "Polyglot", Priority = 4 },
                new() { Id = Guid.NewGuid(), Code = "SPECIALIST", DisplayName = "Specialist", Priority = 4 },
                new() { Id = Guid.NewGuid(), Code = "TEAM_PLAYER", DisplayName = "Team Player", Priority = 6 },
                new() { Id = Guid.NewGuid(), Code = "LONE_WOLF", DisplayName = "Lone Wolf", Priority = 6 },
                new() { Id = Guid.NewGuid(), Code = "VETERAN", DisplayName = "Veteran", Priority = 8 },
                new() { Id = Guid.NewGuid(), Code = "ROOKIE", DisplayName = "Rookie", Priority = 2 },
                new() { Id = Guid.NewGuid(), Code = "PROLIFIC", DisplayName = "Prolific", Priority = 8 },
                new() { Id = Guid.NewGuid(), Code = "CONSISTENT", DisplayName = "Consistent", Priority = 5 },
                new() { Id = Guid.NewGuid(), Code = "COMEBACK_KID", DisplayName = "Comeback Kid", Priority = 7 },
                new() { Id = Guid.NewGuid(), Code = "CROWD_FAVORITE", DisplayName = "Crowd Favorite", Priority = 9 },
                new() { Id = Guid.NewGuid(), Code = "MAINTAINER", DisplayName = "Maintainer", Priority = 6 },
            };

            await context.Badges.AddRangeAsync(badges);
            await context.SaveChangesAsync();
        }
    }
}