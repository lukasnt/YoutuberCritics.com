using Microsoft.EntityFrameworkCore.Migrations;

namespace YoutuberCritics.Migrations
{
    public partial class RenameReviewFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Channels_ChannelFK",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserFK",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ChannelFK",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserFK",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ChannelFK",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserFK",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ChannelID",
                table: "Reviews",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Reviews",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ChannelID",
                table: "Reviews",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews",
                column: "ChannelID",
                principalTable: "Channels",
                principalColumn: "YoutubeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Channels_ChannelID",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_ChannelID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ChannelID",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ChannelFK",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "UserFK",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ChannelFK",
                table: "Reviews",
                column: "ChannelFK");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserFK",
                table: "Reviews",
                column: "UserFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Channels_ChannelFK",
                table: "Reviews",
                column: "ChannelFK",
                principalTable: "Channels",
                principalColumn: "YoutubeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserFK",
                table: "Reviews",
                column: "UserFK",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
