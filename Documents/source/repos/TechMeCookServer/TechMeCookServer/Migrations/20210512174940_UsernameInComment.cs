using Microsoft.EntityFrameworkCore.Migrations;

namespace TechMeCookServer.Migrations
{
    public partial class UsernameInComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "Comments");
        }
    }
}
