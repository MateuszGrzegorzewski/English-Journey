using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FlashCardTablesUpdateRelationshio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardBoxId",
                table: "Flashcards");

            migrationBuilder.AlterColumn<int>(
                name: "FlashcardBoxId",
                table: "Flashcards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardBoxId",
                table: "Flashcards",
                column: "FlashcardBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardBoxId",
                table: "Flashcards");

            migrationBuilder.AlterColumn<int>(
                name: "FlashcardBoxId",
                table: "Flashcards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardBoxId",
                table: "Flashcards",
                column: "FlashcardBoxId",
                unique: true);
        }
    }
}
