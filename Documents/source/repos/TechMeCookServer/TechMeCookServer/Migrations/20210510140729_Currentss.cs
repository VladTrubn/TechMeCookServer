using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechMeCookServer.Migrations
{
    public partial class Currentss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    Ingrid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    image = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    amount = table.Column<double>(nullable: false),
                    unit = table.Column<string>(nullable: true),
                    RecipeRId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredient", x => x.Ingrid);
                    table.ForeignKey(
                        name: "FK_Ingredient_Recipes_RecipeRId",
                        column: x => x.RecipeRId,
                        principalTable: "Recipes",
                        principalColumn: "RId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeRId",
                table: "Ingredient",
                column: "RecipeRId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ingredient");
        }
    }
}
