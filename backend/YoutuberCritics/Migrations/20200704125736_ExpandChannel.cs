using Microsoft.EntityFrameworkCore.Migrations;

namespace YoutuberCritics.Migrations
{
    public partial class ExpandChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels_Copy",
                columns: table => new
                {
                    YoutubeID = table.Column<int>(),
                    YoutubeName = table.Column<string>(maxLength: 50)
                }
            );

            migrationBuilder.Sql("INSERT INTO Channels_Copy(YoutubeID, YoutubeName) SELECT YoutubeID, YoutubeName FROM Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "YoutubeID",
                table: "Channels");

            
            migrationBuilder.AddColumn<int>(
                name: "ChannelID",
                table: "Channels",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropColumn(
                name: "YoutubeName",
                table: "Channels");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Channels",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Channels",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Channels",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubePath",
                table: "Channels",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_YoutubePath",
                table: "Channels",
                column: "YoutubePath",
                unique: true,
                filter: "[YoutubePath] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "ChannelID",
                onDelete: ReferentialAction.Cascade);

                
            migrationBuilder.Sql("INSERT INTO Channels(Title) SELECT YoutubeName FROM Channels_Copy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.DropIndex(
                name: "IX_Channels_YoutubePath",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "YoutubePath",
                table: "Channels");

            migrationBuilder.AddColumn<int>(
                name: "YoutubeID",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "YoutubeName",
                table: "Channels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "YoutubeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "YoutubeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
