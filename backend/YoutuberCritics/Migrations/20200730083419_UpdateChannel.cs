using Microsoft.EntityFrameworkCore.Migrations;

namespace YoutuberCritics.Migrations
{
    public partial class UpdateChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RatingAverage",
                table: "Channels",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewCount",
                table: "Channels",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                    "UPDATE Channels " +
                    "SET Channels.ReviewCount = (SELECT COUNT(*) FROM Reviews WHERE Reviews.ChannelID = Channels.ChannelID), " +
                        "Channels.RatingAverage = (SELECT AVG(Reviews.Rating) FROM Reviews WHERE Reviews.ChannelID = Channels.ChannelID) " +
                    "WHERE ChannelID IN (SELECT DISTINCT Reviews.ChannelID FROM Reviews);"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatingAverage",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ReviewCount",
                table: "Channels");
        }
    }
}
