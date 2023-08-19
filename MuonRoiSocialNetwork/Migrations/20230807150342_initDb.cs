using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class initDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppRole",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    NameCategory = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupUserMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    GroupName = table.Column<string>(type: "text", nullable: false),
                    Roles = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUserMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    TagName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Details = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Story_Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Story_Synopsis = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Img_Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsShow = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TotalView = table.Column<int>(type: "integer", nullable: false),
                    TotalFavorite = table.Column<int>(type: "integer", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    ListRattings = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    AuthorName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Surname = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Email = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Address = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Salt = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastLoginLocation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Avatar = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    LockReason = table.Column<string>(type: "text", nullable: true),
                    GroupId = table.Column<int>(type: "integer", nullable: true),
                    AccountStatus = table.Column<int>(type: "integer", nullable: false),
                    CountRequestSendMail = table.Column<int>(type: "integer", nullable: false),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_GroupUserMember_GroupId",
                        column: x => x.GroupId,
                        principalTable: "GroupUserMember",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Chapter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ChapterTitle = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Body = table.Column<string>(type: "character varying(100000)", maxLength: 100000, nullable: false),
                    NumberOfChapter = table.Column<long>(type: "bigint", nullable: false),
                    NumberOfWord = table.Column<int>(type: "integer", nullable: false),
                    StoryId = table.Column<int>(type: "integer", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chapter_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagInStory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StoryId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagInStory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagInStory_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagInStory_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookMarkStory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StoryGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    BookmarkDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookMarkStory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookMarkStory_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowingAuthor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowingAuthor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FollowingAuthor_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryFavorite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryFavorite", x => new { x.StoryId, x.UserGuid });
                    table.ForeignKey(
                        name: "FK_StoryFavorite_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryFavorite_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    StoryId = table.Column<int>(type: "integer", nullable: false),
                    NotifiUrl = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    NotificationSate = table.Column<int>(type: "integer", nullable: false),
                    ReadNotificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Img_Url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryNotifications_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoryNotifications_Story_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Story",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryPublish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StoryGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryPublish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryPublish_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoryReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedUserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletedUserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdatedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DeletedUserName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    DeletedDateTS = table.Column<double>(type: "double precision", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    StoryGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Content = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoryReview_AppUsers_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: false),
                    KeySalt = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTimeTS = table.Column<double>(type: "double precision", nullable: true),
                    CreateDateTS = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.UserId, x.RefreshToken });
                    table.ForeignKey(
                        name: "FK_UserLogin_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_GroupId",
                table: "AppUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BookMarkStory_UserGuid",
                table: "BookMarkStory",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Chapter_StoryId",
                table: "Chapter",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowingAuthor_UserGuid",
                table: "FollowingAuthor",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Story_CategoryId",
                table: "Story",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryFavorite_UserGuid",
                table: "StoryFavorite",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_StoryNotifications_StoryId",
                table: "StoryNotifications",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoryNotifications_UserGuid",
                table: "StoryNotifications",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_StoryPublish_UserGuid",
                table: "StoryPublish",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_StoryReview_UserGuid",
                table: "StoryReview",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_TagInStory_StoryId",
                table: "TagInStory",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TagInStory_TagId",
                table: "TagInStory",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppRole");

            migrationBuilder.DropTable(
                name: "BookMarkStory");

            migrationBuilder.DropTable(
                name: "Chapter");

            migrationBuilder.DropTable(
                name: "FollowingAuthor");

            migrationBuilder.DropTable(
                name: "StoryFavorite");

            migrationBuilder.DropTable(
                name: "StoryNotifications");

            migrationBuilder.DropTable(
                name: "StoryPublish");

            migrationBuilder.DropTable(
                name: "StoryReview");

            migrationBuilder.DropTable(
                name: "TagInStory");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "GroupUserMember");
        }
    }
}
