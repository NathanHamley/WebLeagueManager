using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLeague.Migrations
{
    public partial class Add_Teams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Season",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamsId",
                table: "Season",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Season_Team_TeamsId",
                table: "Season");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Season_TeamsId",
                table: "Season");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Season");

            migrationBuilder.DropColumn(
                name: "TeamsId",
                table: "Season");
        }
    }
}
