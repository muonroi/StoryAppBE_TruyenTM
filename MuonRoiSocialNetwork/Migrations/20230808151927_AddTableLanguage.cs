using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class AddTableLanguage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_GroupUserMember_GroupId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookMarkStory_AppUsers_UserGuid",
                table: "BookMarkStory");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapter_Story_StoryId",
                table: "Chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowingAuthor_AppUsers_UserGuid",
                table: "FollowingAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_Category_CategoryId",
                table: "Story");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFavorite_AppUsers_UserGuid",
                table: "StoryFavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFavorite_Story_StoryId",
                table: "StoryFavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryNotifications_AppUsers_UserGuid",
                table: "StoryNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryNotifications_Story_StoryId",
                table: "StoryNotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryPublish_AppUsers_UserGuid",
                table: "StoryPublish");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryReview_AppUsers_UserGuid",
                table: "StoryReview");

            migrationBuilder.DropForeignKey(
                name: "FK_TagInStory_Story_StoryId",
                table: "TagInStory");

            migrationBuilder.DropForeignKey(
                name: "FK_TagInStory_Tag_TagId",
                table: "TagInStory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_AppUsers_UserId",
                table: "UserLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagInStory",
                table: "TagInStory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                table: "Tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryReview",
                table: "StoryReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryPublish",
                table: "StoryPublish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryNotifications",
                table: "StoryNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryFavorite",
                table: "StoryFavorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Story",
                table: "Story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupUserMember",
                table: "GroupUserMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowingAuthor",
                table: "FollowingAuthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Chapter",
                table: "Chapter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookMarkStory",
                table: "BookMarkStory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole");

            migrationBuilder.RenameTable(
                name: "UserLogin",
                newName: "userlogin");

            migrationBuilder.RenameTable(
                name: "TagInStory",
                newName: "taginstory");

            migrationBuilder.RenameTable(
                name: "Tag",
                newName: "tag");

            migrationBuilder.RenameTable(
                name: "StoryReview",
                newName: "storyreview");

            migrationBuilder.RenameTable(
                name: "StoryPublish",
                newName: "storypublish");

            migrationBuilder.RenameTable(
                name: "StoryNotifications",
                newName: "storynotifications");

            migrationBuilder.RenameTable(
                name: "StoryFavorite",
                newName: "storyfavorite");

            migrationBuilder.RenameTable(
                name: "Story",
                newName: "story");

            migrationBuilder.RenameTable(
                name: "GroupUserMember",
                newName: "groupusermember");

            migrationBuilder.RenameTable(
                name: "FollowingAuthor",
                newName: "followingauthor");

            migrationBuilder.RenameTable(
                name: "Chapter",
                newName: "chapter");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "category");

            migrationBuilder.RenameTable(
                name: "BookMarkStory",
                newName: "bookmarkstory");

            migrationBuilder.RenameTable(
                name: "AppRole",
                newName: "approle");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTimeTS",
                table: "userlogin",
                newName: "refreshtoken_expirytime_ts");

            migrationBuilder.RenameColumn(
                name: "KeySalt",
                table: "userlogin",
                newName: "key_salf");

            migrationBuilder.RenameColumn(
                name: "CreateDateTS",
                table: "userlogin",
                newName: "create_date_ts");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "userlogin",
                newName: "refresh_token");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "userlogin",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogin_UserId",
                table: "userlogin",
                newName: "IX_userlogin_user_id");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "taginstory",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "taginstory",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "taginstory",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "taginstory",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "taginstory",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "taginstory",
                newName: "tag_id");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "taginstory",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "taginstory",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "taginstory",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "taginstory",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "taginstory",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "taginstory",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "taginstory",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "taginstory",
                newName: "created_date_ts");

            migrationBuilder.RenameIndex(
                name: "IX_TagInStory_TagId",
                table: "taginstory",
                newName: "IX_taginstory_tag_id");

            migrationBuilder.RenameIndex(
                name: "IX_TagInStory_StoryId",
                table: "taginstory",
                newName: "IX_taginstory_story_id");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "tag",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "tag",
                newName: "details");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tag",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "tag",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "tag",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "tag",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "TagName",
                table: "tag",
                newName: "tag_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "tag",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "tag",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "tag",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "tag",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "tag",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "tag",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "tag",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "storyreview",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "storyreview",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "storyreview",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "storyreview",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "storyreview",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "storyreview",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "storyreview",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "storyreview",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryGuid",
                table: "storyreview",
                newName: "story_guid");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "storyreview",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "storyreview",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "storyreview",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "storyreview",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "storyreview",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "storyreview",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "storyreview",
                newName: "created_date_ts");

            migrationBuilder.RenameIndex(
                name: "IX_StoryReview_UserGuid",
                table: "storyreview",
                newName: "IX_storyreview_user_guid");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "storypublish",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "storypublish",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "storypublish",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "storypublish",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "storypublish",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "storypublish",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryGuid",
                table: "storypublish",
                newName: "story_guid");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "storypublish",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "storypublish",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "storypublish",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "storypublish",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "storypublish",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "storypublish",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "storypublish",
                newName: "created_date_ts");

            migrationBuilder.RenameIndex(
                name: "IX_StoryPublish_UserGuid",
                table: "storypublish",
                newName: "IX_storypublish_user_guid");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "storynotifications",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "storynotifications",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Img_Url",
                table: "storynotifications",
                newName: "img_url");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "storynotifications",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "storynotifications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "storynotifications",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "storynotifications",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "storynotifications",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "storynotifications",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "storynotifications",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "ReadNotificationDate",
                table: "storynotifications",
                newName: "read_date");

            migrationBuilder.RenameColumn(
                name: "NotificationSate",
                table: "storynotifications",
                newName: "notification_state");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "storynotifications",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "storynotifications",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "storynotifications",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "storynotifications",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "storynotifications",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "storynotifications",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "storynotifications",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "NotifiUrl",
                table: "storynotifications",
                newName: "notify_url");

            migrationBuilder.RenameIndex(
                name: "IX_StoryNotifications_UserGuid",
                table: "storynotifications",
                newName: "IX_storynotifications_user_guid");

            migrationBuilder.RenameIndex(
                name: "IX_StoryNotifications_StoryId",
                table: "storynotifications",
                newName: "IX_storynotifications_story_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "storyfavorite",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "storyfavorite",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "storyfavorite",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "storyfavorite",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "storyfavorite",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "storyfavorite",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "storyfavorite",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "storyfavorite",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "storyfavorite",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "storyfavorite",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "storyfavorite",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "storyfavorite",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "storyfavorite",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "storyfavorite",
                newName: "story_id");

            migrationBuilder.RenameIndex(
                name: "IX_StoryFavorite_UserGuid",
                table: "storyfavorite",
                newName: "IX_storyfavorite_user_guid");

            migrationBuilder.RenameColumn(
                name: "Story_Title",
                table: "story",
                newName: "story_title");

            migrationBuilder.RenameColumn(
                name: "Story_Synopsis",
                table: "story",
                newName: "story_synopsis");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "story",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "story",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "Img_Url",
                table: "story",
                newName: "img_url");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "story",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "story",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "story",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "story",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "story",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "TotalView",
                table: "story",
                newName: "total_view");

            migrationBuilder.RenameColumn(
                name: "TotalFavorite",
                table: "story",
                newName: "total_favorite");

            migrationBuilder.RenameColumn(
                name: "ListRattings",
                table: "story",
                newName: "list_rattings");

            migrationBuilder.RenameColumn(
                name: "IsShow",
                table: "story",
                newName: "is_show");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "story",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "story",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "story",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "story",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "story",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "story",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "story",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "story",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "AuthorName",
                table: "story",
                newName: "author_name");

            migrationBuilder.RenameIndex(
                name: "IX_Story_CategoryId",
                table: "story",
                newName: "IX_story_category_id");

            migrationBuilder.RenameColumn(
                name: "Roles",
                table: "groupusermember",
                newName: "roles");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "groupusermember",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "groupusermember",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "groupusermember",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "groupusermember",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "groupusermember",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "groupusermember",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "groupusermember",
                newName: "group_name");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "groupusermember",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "groupusermember",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "groupusermember",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "groupusermember",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "groupusermember",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "groupusermember",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "followingauthor",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "followingauthor",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "followingauthor",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "followingauthor",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "followingauthor",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "followingauthor",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryGuid",
                table: "followingauthor",
                newName: "story_guid");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "followingauthor",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "followingauthor",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "followingauthor",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "followingauthor",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "followingauthor",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "followingauthor",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "followingauthor",
                newName: "created_date_ts");

            migrationBuilder.RenameIndex(
                name: "IX_FollowingAuthor_UserGuid",
                table: "followingauthor",
                newName: "IX_followingauthor_user_guid");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "chapter",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "chapter",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "chapter",
                newName: "body");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "chapter",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "chapter",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "chapter",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "chapter",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryId",
                table: "chapter",
                newName: "story_id");

            migrationBuilder.RenameColumn(
                name: "NumberOfWord",
                table: "chapter",
                newName: "number_of_word");

            migrationBuilder.RenameColumn(
                name: "NumberOfChapter",
                table: "chapter",
                newName: "number_of_chapter");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "chapter",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "chapter",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "chapter",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "chapter",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "chapter",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "chapter",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "chapter",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "ChapterTitle",
                table: "chapter",
                newName: "chapter_title");

            migrationBuilder.RenameIndex(
                name: "IX_Chapter_StoryId",
                table: "chapter",
                newName: "IX_chapter_story_id");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "category",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "category",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "category",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "category",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "category",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "NameCategory",
                table: "category",
                newName: "category_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "category",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "category",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "category",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "category",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "category",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "category",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "category",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "category",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "bookmarkstory",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bookmarkstory",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserGuid",
                table: "bookmarkstory",
                newName: "user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "bookmarkstory",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "bookmarkstory",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "bookmarkstory",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "StoryGuid",
                table: "bookmarkstory",
                newName: "story_guid");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "bookmarkstory",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "bookmarkstory",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "bookmarkstory",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "bookmarkstory",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "bookmarkstory",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "bookmarkstory",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "bookmarkstory",
                newName: "created_date_ts");

            migrationBuilder.RenameColumn(
                name: "BookmarkDate",
                table: "bookmarkstory",
                newName: "bookmark_date");

            migrationBuilder.RenameIndex(
                name: "IX_BookMarkStory_UserGuid",
                table: "bookmarkstory",
                newName: "IX_bookmarkstory_user_guid");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AppUsers",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Surname",
                table: "AppUsers",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "AppUsers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "AppUsers",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "AppUsers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "AppUsers",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AppUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "AppUsers",
                newName: "birthdate");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "AppUsers",
                newName: "avatar");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "AppUsers",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "AppUsers",
                newName: "updated_date");

            migrationBuilder.RenameColumn(
                name: "Salt",
                table: "AppUsers",
                newName: "salf");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "AppUsers",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "LockReason",
                table: "AppUsers",
                newName: "lock_reason");

            migrationBuilder.RenameColumn(
                name: "LastLoginLocation",
                table: "AppUsers",
                newName: "last_login_location");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "AppUsers",
                newName: "last_login");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "AppUsers",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "AppUsers",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "AppUsers",
                newName: "deleted_date");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "AppUsers",
                newName: "created_date");

            migrationBuilder.RenameColumn(
                name: "CountRequestSendMail",
                table: "AppUsers",
                newName: "count_request_sendmail");

            migrationBuilder.RenameColumn(
                name: "AccountStatus",
                table: "AppUsers",
                newName: "account_status");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_GroupId",
                table: "AppUsers",
                newName: "IX_AppUsers_group_id");

            migrationBuilder.RenameColumn(
                name: "Guid",
                table: "approle",
                newName: "guid");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "approle",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "approle",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserName",
                table: "approle",
                newName: "updated_username");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserGuid",
                table: "approle",
                newName: "updated_user_guid");

            migrationBuilder.RenameColumn(
                name: "UpdatedDateTS",
                table: "approle",
                newName: "updated_date_ts");

            migrationBuilder.RenameColumn(
                name: "NormalizedName",
                table: "approle",
                newName: "normalized_name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "approle",
                newName: "role_name");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "approle",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "DeletedUserName",
                table: "approle",
                newName: "deleted_username");

            migrationBuilder.RenameColumn(
                name: "DeletedUserGuid",
                table: "approle",
                newName: "deleted_user_guid");

            migrationBuilder.RenameColumn(
                name: "DeletedDateTS",
                table: "approle",
                newName: "deleted_date_ts");

            migrationBuilder.RenameColumn(
                name: "CreatedUserName",
                table: "approle",
                newName: "created_username");

            migrationBuilder.RenameColumn(
                name: "CreatedUserGuid",
                table: "approle",
                newName: "created_user_guid");

            migrationBuilder.RenameColumn(
                name: "CreatedDateTS",
                table: "approle",
                newName: "created_date_ts");

            migrationBuilder.AddColumn<double>(
                name: "created_date_ts",
                table: "userlogin",
                type: "double precision",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 107);

            migrationBuilder.AddColumn<Guid>(
                name: "created_user_guid",
                table: "userlogin",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 101);

            migrationBuilder.AddColumn<string>(
                name: "created_username",
                table: "userlogin",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 104);

            migrationBuilder.AddColumn<double>(
                name: "deleted_date_ts",
                table: "userlogin",
                type: "double precision",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 109);

            migrationBuilder.AddColumn<Guid>(
                name: "deleted_user_guid",
                table: "userlogin",
                type: "uuid",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 103);

            migrationBuilder.AddColumn<string>(
                name: "deleted_username",
                table: "userlogin",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 106);

            migrationBuilder.AddColumn<Guid>(
                name: "guid",
                table: "userlogin",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "userlogin",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_deleted",
                table: "userlogin",
                type: "boolean",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 110);

            migrationBuilder.AddColumn<double>(
                name: "updated_date_ts",
                table: "userlogin",
                type: "double precision",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 108);

            migrationBuilder.AddColumn<Guid>(
                name: "updated_user_guid",
                table: "userlogin",
                type: "uuid",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 102);

            migrationBuilder.AddColumn<string>(
                name: "updated_username",
                table: "userlogin",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 105);

            migrationBuilder.AlterColumn<Guid>(
                name: "guid",
                table: "story",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "groupusermember",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_userlogin",
                table: "userlogin",
                columns: new[] { "user_id", "refresh_token" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_taginstory",
                table: "taginstory",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tag",
                table: "tag",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_storyreview",
                table: "storyreview",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_storypublish",
                table: "storypublish",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_storynotifications",
                table: "storynotifications",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_storyfavorite",
                table: "storyfavorite",
                columns: new[] { "story_id", "user_guid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_story",
                table: "story",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_groupusermember",
                table: "groupusermember",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_followingauthor",
                table: "followingauthor",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chapter",
                table: "chapter",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category",
                table: "category",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookmarkstory",
                table: "bookmarkstory",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_approle",
                table: "approle",
                column: "id");

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    lang = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.lang);
                });

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
                name: "FK_chapter_story_story_id",
                table: "chapter",
                column: "story_id",
                principalTable: "story",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_followingauthor_AppUsers_user_guid",
                table: "followingauthor",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_story_category_category_id",
                table: "story",
                column: "category_id",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyfavorite_AppUsers_user_guid",
                table: "storyfavorite",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storyfavorite_story_story_id",
                table: "storyfavorite",
                column: "story_id",
                principalTable: "story",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storynotifications_AppUsers_user_guid",
                table: "storynotifications",
                column: "user_guid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_storynotifications_story_story_id",
                table: "storynotifications",
                column: "story_id",
                principalTable: "story",
                principalColumn: "id",
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
                name: "FK_taginstory_story_story_id",
                table: "taginstory",
                column: "story_id",
                principalTable: "story",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_taginstory_tag_tag_id",
                table: "taginstory",
                column: "tag_id",
                principalTable: "tag",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userlogin_AppUsers_user_id",
                table: "userlogin",
                column: "user_id",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_groupusermember_group_id",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_bookmarkstory_AppUsers_user_guid",
                table: "bookmarkstory");

            migrationBuilder.DropForeignKey(
                name: "FK_chapter_story_story_id",
                table: "chapter");

            migrationBuilder.DropForeignKey(
                name: "FK_followingauthor_AppUsers_user_guid",
                table: "followingauthor");

            migrationBuilder.DropForeignKey(
                name: "FK_story_category_category_id",
                table: "story");

            migrationBuilder.DropForeignKey(
                name: "FK_storyfavorite_AppUsers_user_guid",
                table: "storyfavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_storyfavorite_story_story_id",
                table: "storyfavorite");

            migrationBuilder.DropForeignKey(
                name: "FK_storynotifications_AppUsers_user_guid",
                table: "storynotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_storynotifications_story_story_id",
                table: "storynotifications");

            migrationBuilder.DropForeignKey(
                name: "FK_storypublish_AppUsers_user_guid",
                table: "storypublish");

            migrationBuilder.DropForeignKey(
                name: "FK_storyreview_AppUsers_user_guid",
                table: "storyreview");

            migrationBuilder.DropForeignKey(
                name: "FK_taginstory_story_story_id",
                table: "taginstory");

            migrationBuilder.DropForeignKey(
                name: "FK_taginstory_tag_tag_id",
                table: "taginstory");

            migrationBuilder.DropForeignKey(
                name: "FK_userlogin_AppUsers_user_id",
                table: "userlogin");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userlogin",
                table: "userlogin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_taginstory",
                table: "taginstory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tag",
                table: "tag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_storyreview",
                table: "storyreview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_storypublish",
                table: "storypublish");

            migrationBuilder.DropPrimaryKey(
                name: "PK_storynotifications",
                table: "storynotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_storyfavorite",
                table: "storyfavorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_story",
                table: "story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_groupusermember",
                table: "groupusermember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_followingauthor",
                table: "followingauthor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chapter",
                table: "chapter");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category",
                table: "category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookmarkstory",
                table: "bookmarkstory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_approle",
                table: "approle");

            migrationBuilder.DropColumn(
                name: "created_date_ts",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "created_user_guid",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "created_username",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "deleted_date_ts",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "deleted_user_guid",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "deleted_username",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "guid",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "id",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "is_deleted",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "updated_date_ts",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "updated_user_guid",
                table: "userlogin");

            migrationBuilder.DropColumn(
                name: "updated_username",
                table: "userlogin");

            migrationBuilder.RenameTable(
                name: "userlogin",
                newName: "UserLogin");

            migrationBuilder.RenameTable(
                name: "taginstory",
                newName: "TagInStory");

            migrationBuilder.RenameTable(
                name: "tag",
                newName: "Tag");

            migrationBuilder.RenameTable(
                name: "storyreview",
                newName: "StoryReview");

            migrationBuilder.RenameTable(
                name: "storypublish",
                newName: "StoryPublish");

            migrationBuilder.RenameTable(
                name: "storynotifications",
                newName: "StoryNotifications");

            migrationBuilder.RenameTable(
                name: "storyfavorite",
                newName: "StoryFavorite");

            migrationBuilder.RenameTable(
                name: "story",
                newName: "Story");

            migrationBuilder.RenameTable(
                name: "groupusermember",
                newName: "GroupUserMember");

            migrationBuilder.RenameTable(
                name: "followingauthor",
                newName: "FollowingAuthor");

            migrationBuilder.RenameTable(
                name: "chapter",
                newName: "Chapter");

            migrationBuilder.RenameTable(
                name: "category",
                newName: "Category");

            migrationBuilder.RenameTable(
                name: "bookmarkstory",
                newName: "BookMarkStory");

            migrationBuilder.RenameTable(
                name: "approle",
                newName: "AppRole");

            migrationBuilder.RenameColumn(
                name: "refreshtoken_expirytime_ts",
                table: "UserLogin",
                newName: "RefreshTokenExpiryTimeTS");

            migrationBuilder.RenameColumn(
                name: "key_salf",
                table: "UserLogin",
                newName: "KeySalt");

            migrationBuilder.RenameColumn(
                name: "create_date_ts",
                table: "UserLogin",
                newName: "CreateDateTS");

            migrationBuilder.RenameColumn(
                name: "refresh_token",
                table: "UserLogin",
                newName: "RefreshToken");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "UserLogin",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_userlogin_user_id",
                table: "UserLogin",
                newName: "IX_UserLogin_UserId");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "TagInStory",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TagInStory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "TagInStory",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "TagInStory",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "TagInStory",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "TagInStory",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "TagInStory",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "TagInStory",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "TagInStory",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "TagInStory",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "TagInStory",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "TagInStory",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "TagInStory",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "TagInStory",
                newName: "CreatedDateTS");

            migrationBuilder.RenameIndex(
                name: "IX_taginstory_tag_id",
                table: "TagInStory",
                newName: "IX_TagInStory_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_taginstory_story_id",
                table: "TagInStory",
                newName: "IX_TagInStory_StoryId");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Tag",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "details",
                table: "Tag",
                newName: "Details");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tag",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "Tag",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "Tag",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "Tag",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "tag_name",
                table: "Tag",
                newName: "TagName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Tag",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "Tag",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "Tag",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "Tag",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "Tag",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "Tag",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "Tag",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "StoryReview",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "StoryReview",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "StoryReview",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StoryReview",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "StoryReview",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "StoryReview",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "StoryReview",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "StoryReview",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_guid",
                table: "StoryReview",
                newName: "StoryGuid");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StoryReview",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "StoryReview",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "StoryReview",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "StoryReview",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "StoryReview",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "StoryReview",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "StoryReview",
                newName: "CreatedDateTS");

            migrationBuilder.RenameIndex(
                name: "IX_storyreview_user_guid",
                table: "StoryReview",
                newName: "IX_StoryReview_UserGuid");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "StoryPublish",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StoryPublish",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "StoryPublish",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "StoryPublish",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "StoryPublish",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "StoryPublish",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_guid",
                table: "StoryPublish",
                newName: "StoryGuid");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StoryPublish",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "StoryPublish",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "StoryPublish",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "StoryPublish",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "StoryPublish",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "StoryPublish",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "StoryPublish",
                newName: "CreatedDateTS");

            migrationBuilder.RenameIndex(
                name: "IX_storypublish_user_guid",
                table: "StoryPublish",
                newName: "IX_StoryPublish_UserGuid");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "StoryNotifications",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "StoryNotifications",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "img_url",
                table: "StoryNotifications",
                newName: "Img_Url");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "StoryNotifications",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StoryNotifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "StoryNotifications",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "StoryNotifications",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "StoryNotifications",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "StoryNotifications",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "StoryNotifications",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "read_date",
                table: "StoryNotifications",
                newName: "ReadNotificationDate");

            migrationBuilder.RenameColumn(
                name: "notification_state",
                table: "StoryNotifications",
                newName: "NotificationSate");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StoryNotifications",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "StoryNotifications",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "StoryNotifications",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "StoryNotifications",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "StoryNotifications",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "StoryNotifications",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "StoryNotifications",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "notify_url",
                table: "StoryNotifications",
                newName: "NotifiUrl");

            migrationBuilder.RenameIndex(
                name: "IX_storynotifications_user_guid",
                table: "StoryNotifications",
                newName: "IX_StoryNotifications_UserGuid");

            migrationBuilder.RenameIndex(
                name: "IX_storynotifications_story_id",
                table: "StoryNotifications",
                newName: "IX_StoryNotifications_StoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StoryFavorite",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "StoryFavorite",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "StoryFavorite",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "StoryFavorite",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "StoryFavorite",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "StoryFavorite",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "StoryFavorite",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "StoryFavorite",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "StoryFavorite",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "StoryFavorite",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "StoryFavorite",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "StoryFavorite",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "StoryFavorite",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "StoryFavorite",
                newName: "StoryId");

            migrationBuilder.RenameIndex(
                name: "IX_storyfavorite_user_guid",
                table: "StoryFavorite",
                newName: "IX_StoryFavorite_UserGuid");

            migrationBuilder.RenameColumn(
                name: "story_title",
                table: "Story",
                newName: "Story_Title");

            migrationBuilder.RenameColumn(
                name: "story_synopsis",
                table: "Story",
                newName: "Story_Synopsis");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Story",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Story",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "img_url",
                table: "Story",
                newName: "Img_Url");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Story",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Story",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "Story",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "Story",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "Story",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "total_view",
                table: "Story",
                newName: "TotalView");

            migrationBuilder.RenameColumn(
                name: "total_favorite",
                table: "Story",
                newName: "TotalFavorite");

            migrationBuilder.RenameColumn(
                name: "list_rattings",
                table: "Story",
                newName: "ListRattings");

            migrationBuilder.RenameColumn(
                name: "is_show",
                table: "Story",
                newName: "IsShow");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Story",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "Story",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "Story",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "Story",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "Story",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "Story",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "Story",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Story",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "author_name",
                table: "Story",
                newName: "AuthorName");

            migrationBuilder.RenameIndex(
                name: "IX_story_category_id",
                table: "Story",
                newName: "IX_Story_CategoryId");

            migrationBuilder.RenameColumn(
                name: "roles",
                table: "GroupUserMember",
                newName: "Roles");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "GroupUserMember",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "GroupUserMember",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "GroupUserMember",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "GroupUserMember",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "GroupUserMember",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "GroupUserMember",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "group_name",
                table: "GroupUserMember",
                newName: "GroupName");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "GroupUserMember",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "GroupUserMember",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "GroupUserMember",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "GroupUserMember",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "GroupUserMember",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "GroupUserMember",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "FollowingAuthor",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "FollowingAuthor",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "FollowingAuthor",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "FollowingAuthor",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "FollowingAuthor",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "FollowingAuthor",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_guid",
                table: "FollowingAuthor",
                newName: "StoryGuid");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "FollowingAuthor",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "FollowingAuthor",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "FollowingAuthor",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "FollowingAuthor",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "FollowingAuthor",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "FollowingAuthor",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "FollowingAuthor",
                newName: "CreatedDateTS");

            migrationBuilder.RenameIndex(
                name: "IX_followingauthor_user_guid",
                table: "FollowingAuthor",
                newName: "IX_FollowingAuthor_UserGuid");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Chapter",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Chapter",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "body",
                table: "Chapter",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Chapter",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "Chapter",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "Chapter",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "Chapter",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_id",
                table: "Chapter",
                newName: "StoryId");

            migrationBuilder.RenameColumn(
                name: "number_of_word",
                table: "Chapter",
                newName: "NumberOfWord");

            migrationBuilder.RenameColumn(
                name: "number_of_chapter",
                table: "Chapter",
                newName: "NumberOfChapter");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Chapter",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "Chapter",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "Chapter",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "Chapter",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "Chapter",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "Chapter",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "Chapter",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "chapter_title",
                table: "Chapter",
                newName: "ChapterTitle");

            migrationBuilder.RenameIndex(
                name: "IX_chapter_story_id",
                table: "Chapter",
                newName: "IX_Chapter_StoryId");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "Category",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "Category",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "Category",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "Category",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Category",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Category",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "Category",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "Category",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "Category",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "Category",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "Category",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "Category",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "category_name",
                table: "Category",
                newName: "NameCategory");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "BookMarkStory",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "BookMarkStory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_guid",
                table: "BookMarkStory",
                newName: "UserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "BookMarkStory",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "BookMarkStory",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "BookMarkStory",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "story_guid",
                table: "BookMarkStory",
                newName: "StoryGuid");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "BookMarkStory",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "BookMarkStory",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "BookMarkStory",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "BookMarkStory",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "BookMarkStory",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "BookMarkStory",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "BookMarkStory",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "bookmark_date",
                table: "BookMarkStory",
                newName: "BookmarkDate");

            migrationBuilder.RenameIndex(
                name: "IX_bookmarkstory_user_guid",
                table: "BookMarkStory",
                newName: "IX_BookMarkStory_UserGuid");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "AppUsers",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "AppUsers",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "AppUsers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "AppUsers",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "AppUsers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "AppUsers",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AppUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "birthdate",
                table: "AppUsers",
                newName: "BirthDate");

            migrationBuilder.RenameColumn(
                name: "avatar",
                table: "AppUsers",
                newName: "Avatar");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "AppUsers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "updated_date",
                table: "AppUsers",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "salf",
                table: "AppUsers",
                newName: "Salt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "AppUsers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "lock_reason",
                table: "AppUsers",
                newName: "LockReason");

            migrationBuilder.RenameColumn(
                name: "last_login_location",
                table: "AppUsers",
                newName: "LastLoginLocation");

            migrationBuilder.RenameColumn(
                name: "last_login",
                table: "AppUsers",
                newName: "LastLogin");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "AppUsers",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "AppUsers",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "deleted_date",
                table: "AppUsers",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_date",
                table: "AppUsers",
                newName: "CreatedDateTS");

            migrationBuilder.RenameColumn(
                name: "count_request_sendmail",
                table: "AppUsers",
                newName: "CountRequestSendMail");

            migrationBuilder.RenameColumn(
                name: "account_status",
                table: "AppUsers",
                newName: "AccountStatus");

            migrationBuilder.RenameIndex(
                name: "IX_AppUsers_group_id",
                table: "AppUsers",
                newName: "IX_AppUsers_GroupId");

            migrationBuilder.RenameColumn(
                name: "guid",
                table: "AppRole",
                newName: "Guid");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "AppRole",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AppRole",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_username",
                table: "AppRole",
                newName: "UpdatedUserName");

            migrationBuilder.RenameColumn(
                name: "updated_user_guid",
                table: "AppRole",
                newName: "UpdatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "updated_date_ts",
                table: "AppRole",
                newName: "UpdatedDateTS");

            migrationBuilder.RenameColumn(
                name: "role_name",
                table: "AppRole",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "normalized_name",
                table: "AppRole",
                newName: "NormalizedName");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "AppRole",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "deleted_username",
                table: "AppRole",
                newName: "DeletedUserName");

            migrationBuilder.RenameColumn(
                name: "deleted_user_guid",
                table: "AppRole",
                newName: "DeletedUserGuid");

            migrationBuilder.RenameColumn(
                name: "deleted_date_ts",
                table: "AppRole",
                newName: "DeletedDateTS");

            migrationBuilder.RenameColumn(
                name: "created_username",
                table: "AppRole",
                newName: "CreatedUserName");

            migrationBuilder.RenameColumn(
                name: "created_user_guid",
                table: "AppRole",
                newName: "CreatedUserGuid");

            migrationBuilder.RenameColumn(
                name: "created_date_ts",
                table: "AppRole",
                newName: "CreatedDateTS");

            migrationBuilder.AlterColumn<Guid>(
                name: "Guid",
                table: "Story",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "GroupUserMember",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .Annotation("Relational:ColumnOrder", 0)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogin",
                table: "UserLogin",
                columns: new[] { "UserId", "RefreshToken" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagInStory",
                table: "TagInStory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryReview",
                table: "StoryReview",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryPublish",
                table: "StoryPublish",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryNotifications",
                table: "StoryNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryFavorite",
                table: "StoryFavorite",
                columns: new[] { "StoryId", "UserGuid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Story",
                table: "Story",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupUserMember",
                table: "GroupUserMember",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowingAuthor",
                table: "FollowingAuthor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Chapter",
                table: "Chapter",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookMarkStory",
                table: "BookMarkStory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRole",
                table: "AppRole",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_GroupUserMember_GroupId",
                table: "AppUsers",
                column: "GroupId",
                principalTable: "GroupUserMember",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookMarkStory_AppUsers_UserGuid",
                table: "BookMarkStory",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapter_Story_StoryId",
                table: "Chapter",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowingAuthor_AppUsers_UserGuid",
                table: "FollowingAuthor",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Story_Category_CategoryId",
                table: "Story",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFavorite_AppUsers_UserGuid",
                table: "StoryFavorite",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFavorite_Story_StoryId",
                table: "StoryFavorite",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryNotifications_AppUsers_UserGuid",
                table: "StoryNotifications",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryNotifications_Story_StoryId",
                table: "StoryNotifications",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryPublish_AppUsers_UserGuid",
                table: "StoryPublish",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryReview_AppUsers_UserGuid",
                table: "StoryReview",
                column: "UserGuid",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagInStory_Story_StoryId",
                table: "TagInStory",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TagInStory_Tag_TagId",
                table: "TagInStory",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_AppUsers_UserId",
                table: "UserLogin",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
