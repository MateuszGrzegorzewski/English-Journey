using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FlashcardsCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ConnectionTopics",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserId",
                table: "Notes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardsCategories_UserId",
                table: "FlashcardsCategories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionTopics_UserId",
                table: "ConnectionTopics",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectionTopics_AspNetUsers_UserId",
                table: "ConnectionTopics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlashcardsCategories_AspNetUsers_UserId",
                table: "FlashcardsCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectionTopics_AspNetUsers_UserId",
                table: "ConnectionTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_FlashcardsCategories_AspNetUsers_UserId",
                table: "FlashcardsCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_UserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_UserId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_FlashcardsCategories_UserId",
                table: "FlashcardsCategories");

            migrationBuilder.DropIndex(
                name: "IX_ConnectionTopics_UserId",
                table: "ConnectionTopics");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlashcardsCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConnectionTopics");
        }
    }
}
