using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviorInRelationshipBetweenFlaschards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardsBoxes_FlashcardsCategories_FlashcardCategoryId",
                table: "FlashcardsBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardsBoxes_FlashcardsCategories_FlashcardCategoryId",
                table: "FlashcardsBoxes",
                column: "FlashcardCategoryId",
                principalTable: "FlashcardsCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardsBoxes_FlashcardsCategories_FlashcardCategoryId",
                table: "FlashcardsBoxes");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardsBoxes_FlashcardsCategories_FlashcardCategoryId",
                table: "FlashcardsBoxes",
                column: "FlashcardCategoryId",
                principalTable: "FlashcardsCategories",
                principalColumn: "Id");
        }
    }
}
