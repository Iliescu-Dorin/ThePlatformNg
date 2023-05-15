using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamData.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDreamAndInterpretation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dream",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Symbols = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dream", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interpretation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtractedText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedText_StartOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SelectedText_EndOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Culture = table.Column<int>(type: "int", nullable: false),
                    DreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interpretation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interpretation_Dream_DreamId",
                        column: x => x.DreamId,
                        principalTable: "Dream",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Interpretation_DreamId",
                table: "Interpretation",
                column: "DreamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interpretation");

            migrationBuilder.DropTable(
                name: "Dream");
        }
    }
}
