using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GitFormula_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GitHubProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    PrimaryLanguage = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AccountCreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CacheExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GitHubProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DuelRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileAId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileBId = table.Column<Guid>(type: "uuid", nullable: false),
                    WinnerProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProfileAScore = table.Column<int>(type: "integer", nullable: false),
                    ProfileBScore = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShareSlug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuelRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuelRecords_GitHubProfiles_ProfileAId",
                        column: x => x.ProfileAId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuelRecords_GitHubProfiles_ProfileBId",
                        column: x => x.ProfileBId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuelRecords_GitHubProfiles_WinnerProfileId",
                        column: x => x.WinnerProfileId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProfileBadges",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileBadges", x => new { x.ProfileId, x.BadgeId });
                    table.ForeignKey(
                        name: "FK_ProfileBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileBadges_GitHubProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RawGitHubStats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CommitsLastYear = table.Column<int>(type: "integer", nullable: false),
                    MergedPullRequests = table.Column<int>(type: "integer", nullable: false),
                    CodeReviews = table.Column<int>(type: "integer", nullable: false),
                    IssuesClosed = table.Column<int>(type: "integer", nullable: false),
                    CollabContributions = table.Column<int>(type: "integer", nullable: false),
                    LifetimeContributions = table.Column<int>(type: "integer", nullable: false),
                    LongestStreakDays = table.Column<int>(type: "integer", nullable: false),
                    CurrentStreakDays = table.Column<int>(type: "integer", nullable: false),
                    Followers = table.Column<int>(type: "integer", nullable: false),
                    StarsEarned = table.Column<int>(type: "integer", nullable: false),
                    TopRepoStars = table.Column<int>(type: "integer", nullable: false),
                    LanguageBreakdownJson = table.Column<string>(type: "text", nullable: false),
                    RawContributionCalendarJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawGitHubStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawGitHubStats_GitHubProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreCards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    Pace = table.Column<int>(type: "integer", nullable: false),
                    Offense = table.Column<int>(type: "integer", nullable: false),
                    Defense = table.Column<int>(type: "integer", nullable: false),
                    Racecraft = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    Mentality = table.Column<int>(type: "integer", nullable: false),
                    Overall = table.Column<int>(type: "integer", nullable: false),
                    ComputedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AlgorithmVersion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreCards_GitHubProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "GitHubProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuelRecords_ProfileAId",
                table: "DuelRecords",
                column: "ProfileAId");

            migrationBuilder.CreateIndex(
                name: "IX_DuelRecords_ProfileBId",
                table: "DuelRecords",
                column: "ProfileBId");

            migrationBuilder.CreateIndex(
                name: "IX_DuelRecords_WinnerProfileId",
                table: "DuelRecords",
                column: "WinnerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_GitHubProfiles_Username",
                table: "GitHubProfiles",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileBadges_BadgeId",
                table: "ProfileBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_RawGitHubStats_ProfileId",
                table: "RawGitHubStats",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCards_ProfileId",
                table: "ScoreCards",
                column: "ProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuelRecords");

            migrationBuilder.DropTable(
                name: "ProfileBadges");

            migrationBuilder.DropTable(
                name: "RawGitHubStats");

            migrationBuilder.DropTable(
                name: "ScoreCards");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "GitHubProfiles");
        }
    }
}
