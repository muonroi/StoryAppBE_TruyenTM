using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class AddTypeNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "read_date",
                table: "storynotifications");

            migrationBuilder.AddColumn<int>(
                name: "notification_type",
                table: "storynotifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notification_type",
                table: "storynotifications");

            migrationBuilder.AddColumn<DateTime>(
                name: "read_date",
                table: "storynotifications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
