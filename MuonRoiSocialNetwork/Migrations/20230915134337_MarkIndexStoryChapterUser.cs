using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class MarkIndexStoryChapterUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_tag_tag_name_id_guid",
                table: "tag",
                columns: new[] { "tag_name", "id", "guid" });

            migrationBuilder.CreateIndex(
                name: "IX_story_story_title_author_name_id_guid_category_id",
                table: "story",
                columns: new[] { "story_title", "author_name", "id", "guid", "category_id" });

            migrationBuilder.CreateIndex(
                name: "IX_chapter_chapter_title_number_of_chapter_id_guid",
                table: "chapter",
                columns: new[] { "chapter_title", "number_of_chapter", "id", "guid" });

            migrationBuilder.CreateIndex(
                name: "IX_category_id_guid_category_name",
                table: "category",
                columns: new[] { "id", "guid", "category_name" });

            migrationBuilder.CreateIndex(
                name: "IX_appuser_username_Id",
                table: "appuser",
                columns: new[] { "username", "Id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tag_tag_name_id_guid",
                table: "tag");

            migrationBuilder.DropIndex(
                name: "IX_story_story_title_author_name_id_guid_category_id",
                table: "story");

            migrationBuilder.DropIndex(
                name: "IX_chapter_chapter_title_number_of_chapter_id_guid",
                table: "chapter");

            migrationBuilder.DropIndex(
                name: "IX_category_id_guid_category_name",
                table: "category");

            migrationBuilder.DropIndex(
                name: "IX_appuser_username_Id",
                table: "appuser");

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
    }
}
