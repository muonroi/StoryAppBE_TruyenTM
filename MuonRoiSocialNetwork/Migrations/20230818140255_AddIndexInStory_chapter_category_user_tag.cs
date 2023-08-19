using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class AddIndexInStory_chapter_category_user_tag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tag_tag_name",
                table: "tag",
                column: "tag_name");

            migrationBuilder.CreateIndex(
                name: "IX_story_story_title_author_name",
                table: "story",
                columns: new[] { "story_title", "author_name" });

            migrationBuilder.CreateIndex(
                name: "IX_chapter_chapter_title_number_of_chapter",
                table: "chapter",
                columns: new[] { "chapter_title", "number_of_chapter" });

            migrationBuilder.CreateIndex(
                name: "IX_category_category_name",
                table: "category",
                column: "category_name");

            migrationBuilder.CreateIndex(
                name: "IX_appuser_username",
                table: "appuser",
                column: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tag_tag_name",
                table: "tag");

            migrationBuilder.DropIndex(
                name: "IX_story_story_title_author_name",
                table: "story");

            migrationBuilder.DropIndex(
                name: "IX_chapter_chapter_title_number_of_chapter",
                table: "chapter");

            migrationBuilder.DropIndex(
                name: "IX_category_category_name",
                table: "category");

            migrationBuilder.DropIndex(
                name: "IX_appuser_username",
                table: "appuser");
        }
    }
}
