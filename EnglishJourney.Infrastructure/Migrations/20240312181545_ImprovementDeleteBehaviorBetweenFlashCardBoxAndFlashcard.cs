using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImprovementDeleteBehaviorBetweenFlashCardBoxAndFlashcard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardsBoxes_FlashcardBoxId",
                table: "Flashcards");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardsBoxes_FlashcardBoxId",
                table: "Flashcards",
                column: "FlashcardBoxId",
                principalTable: "FlashcardsBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardsBoxes_FlashcardBoxId",
                table: "Flashcards");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardsBoxes_FlashcardBoxId",
                table: "Flashcards",
                column: "FlashcardBoxId",
                principalTable: "FlashcardsBoxes",
                principalColumn: "Id");
        }
    }
}
