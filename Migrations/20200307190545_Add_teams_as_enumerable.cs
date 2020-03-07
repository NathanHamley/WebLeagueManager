using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLeague.Migrations
{
    public partial class Add_teams_as_enumerable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Season_Team_TeamsId",
                table: "Season");

            migrationBuilder.DropIndex(
                name: "IX_Season_TeamsId",
                table: "Season");

            migrationBuilder.DropColumn(
                name: "TeamsId",
                table: "Season");

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Team",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Team_SeasonId",
                table: "Team",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_Season_SeasonId",
                table: "Team",
                column: "SeasonId",
                principalTable: "Season",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_Season_SeasonId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_SeasonId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Team");

            migrationBuilder.AddColumn<int>(
                name: "TeamsId",
                table: "Season",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Season_TeamsId",
                table: "Season",
                column: "TeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Season_Team_TeamsId",
                table: "Season",
                column: "TeamsId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
