using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frontend.Migrations
{
    /// <inheritdoc />
    public partial class US_C_Implementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "Simulation_history",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Simulation_history",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Class_Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Challenge_id = table.Column<int>(type: "int", nullable: false),
                    Class_id = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_Challenges_Challenges_Challenge_id",
                        column: x => x.Challenge_id,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Class_Challenges_Classes_Class_id",
                        column: x => x.Class_id,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Class_Challenges_Challenge_id",
                table: "Class_Challenges",
                column: "Challenge_id");

            migrationBuilder.CreateIndex(
                name: "IX_Class_Challenges_Class_id",
                table: "Class_Challenges",
                column: "Class_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Class_Challenges");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "Simulation_history");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Simulation_history");
        }
    }
}
