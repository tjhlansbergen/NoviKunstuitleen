using Microsoft.EntityFrameworkCore.Migrations;

namespace NoviKunstuitleen.Migrations
{
    public partial class imagefiletype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "NoviArtCollection",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "NoviArtCollection");
        }
    }
}
