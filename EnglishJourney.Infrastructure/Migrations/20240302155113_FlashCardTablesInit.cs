using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlashCardTablesInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardsCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardsCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlashcardsBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoxNumber = table.Column<int>(type: "int", nullable: false),
                    FlashcardCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardsBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardsBoxes_FlashcardsCategories_FlashcardCategoryId",
                        column: x => x.FlashcardCategoryId,
                        principalTable: "FlashcardsCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phrase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastGuessed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FlashcardBoxId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_FlashcardsBoxes_FlashcardBoxId",
                        column: x => x.FlashcardBoxId,
                        principalTable: "FlashcardsBoxes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardBoxId",
                table: "Flashcards",
                column: "FlashcardBoxId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardsBoxes_FlashcardCategoryId",
                table: "FlashcardsBoxes",
                column: "FlashcardCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardsBoxes");

            migrationBuilder.DropTable(
                name: "FlashcardsCategories");
        }
    }
}
