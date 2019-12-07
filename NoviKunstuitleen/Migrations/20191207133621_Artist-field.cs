using Microsoft.EntityFrameworkCore.Migrations;

namespace NoviKunstuitleen.Migrations
{
    public partial class Artistfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Artist",
                table: "NoviArtCollection",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Artist",
                table: "NoviArtCollection");
        }
    }
}
