using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MuonRoiSocialNetwork.Migrations
{
    public partial class AddColumnIconCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icon",
                table: "category");
        }
    }
}
