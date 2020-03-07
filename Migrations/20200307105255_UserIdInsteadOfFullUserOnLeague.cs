using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLeague.Migrations
{
    public partial class UserIdInsteadOfFullUserOnLeague : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_League_Users_UserId",
                table: "League");

            migrationBuilder.DropIndex(
                name: "IX_League_UserId",
                table: "League");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "League",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "League",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_League_UserId",
                table: "League",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_League_Users_UserId",
                table: "League",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
