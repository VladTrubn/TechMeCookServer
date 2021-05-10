using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechMeCookServer.Migrations
{
    public partial class Current : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instruction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    RecipeRId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruction_Recipes_RecipeRId",
                        column: x => x.RecipeRId,
                        principalTable: "Recipes",
                        principalColumn: "RId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstructionStep",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    number = table.Column<int>(nullable: false),
                    step = table.Column<string>(nullable: true),
                    InstructionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionStep", x => x.id);
                    table.ForeignKey(
                        name: "FK_InstructionStep_Instruction_InstructionId",
                        column: x => x.InstructionId,
                        principalTable: "Instruction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instruction_RecipeRId",
                table: "Instruction",
                column: "RecipeRId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructionStep_InstructionId",
                table: "InstructionStep",
                column: "InstructionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructionStep");

            migrationBuilder.DropTable(
                name: "Instruction");
        }
    }
}
