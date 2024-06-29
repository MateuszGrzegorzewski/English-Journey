using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishJourney.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConnectionTopicTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopicImage",
                table: "ConnectionTopics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TopicImage",
                table: "ConnectionTopics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
