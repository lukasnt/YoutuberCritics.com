using Microsoft.EntityFrameworkCore.Migrations;

namespace YoutuberCritics.Migrations
{
    public partial class AddSubscribers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Subscribers",
                table: "Channels",
                nullable: true,
                defaultValue: null);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subscribers",
                table: "Channels");
        }
    }
}
