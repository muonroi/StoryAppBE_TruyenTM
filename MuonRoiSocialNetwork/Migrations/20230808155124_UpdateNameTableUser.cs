using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class UpdateNameTableUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_groupusermember_group_id",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_bookmarkstory_AppUsers_user_guid",
                table: "bookmarkstory");

            migrationBuilder.DropForeignKey(
                name: "FK_followingauthor_AppUsers_user_guid",
                table: "followingauthor");

            migrationBuilder.DropForeignKey(
                name: "FK_storyfavorite_AppUsers_user_guid",
                table: "storyfavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_storynotifications_AppUsers_user_guid",
                table: "storynotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_storypublish_AppUsers_user_guid",
                table: "storypublish");

            migrationBuilder.DropForeignKey(
                name: "FK_storyreview_AppUsers_user_guid",
                table: "storyreview");

            migrationBuilder.DropForeignKey(
                name: "FK_userlogin_AppUsers_user_id",
                table: "userlogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "appuser");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_group_id",
                table: "appuser",
                newName: "IX_appuser_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_appuser",
                table: "appuser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_appuser_groupusermember_group_id",
                table: "appuser",
                column: "group_id",
                principalTable: "groupusermember",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookmarkstory_appuser_user_guid",
                table: "bookmarkstory",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_followingauthor_appuser_user_guid",
                table: "followingauthor",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyfavorite_appuser_user_guid",
                table: "storyfavorite",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storynotifications_appuser_user_guid",
                table: "storynotifications",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storypublish_appuser_user_guid",
                table: "storypublish",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyreview_appuser_user_guid",
                table: "storyreview",
                column: "user_guid",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userlogin_appuser_user_id",
                table: "userlogin",
                column: "user_id",
                principalTable: "appuser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appuser_groupusermember_group_id",
                table: "appuser");

            migrationBuilder.DropForeignKey(
                name: "FK_bookmarkstory_appuser_user_guid",
                table: "bookmarkstory");

            migrationBuilder.DropForeignKey(
                name: "FK_followingauthor_appuser_user_guid",
                table: "followingauthor");

            migrationBuilder.DropForeignKey(
                name: "FK_storyfavorite_appuser_user_guid",
                table: "storyfavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_storynotifications_appuser_user_guid",
                table: "storynotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_storypublish_appuser_user_guid",
                table: "storypublish");

            migrationBuilder.DropForeignKey(
                name: "FK_storyreview_appuser_user_guid",
                table: "storyreview");

            migrationBuilder.DropForeignKey(
                name: "FK_userlogin_appuser_user_id",
                table: "userlogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_appuser",
                table: "appuser");

            migrationBuilder.RenameTable(
                name: "appuser",
                newName: "AppUsers");

            migrationBuilder.RenameIndex(
                name: "IX_appuser_group_id",
                table: "AppUsers",
                newName: "IX_AppUsers_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_groupusermember_group_id",
                table: "AppUsers",
                column: "group_id",
                principalTable: "groupusermember",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookmarkstory_AppUsers_user_guid",
                table: "bookmarkstory",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_followingauthor_AppUsers_user_guid",
                table: "followingauthor",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyfavorite_AppUsers_user_guid",
                table: "storyfavorite",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storynotifications_AppUsers_user_guid",
                table: "storynotifications",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storypublish_AppUsers_user_guid",
                table: "storypublish",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyreview_AppUsers_user_guid",
                table: "storyreview",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userlogin_AppUsers_user_id",
                table: "userlogin",
                column: "user_id",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
