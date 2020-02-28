using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dyno.Migrations
{
    public partial class Ninethasasdaasda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connection_Users_FriendaId",
                table: "Connection");

            migrationBuilder.DropForeignKey(
                name: "FK_Connection_Users_FriendbId",
                table: "Connection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connection",
                table: "Connection");

            migrationBuilder.RenameTable(
                name: "Connection",
                newName: "Connections");

            migrationBuilder.RenameIndex(
                name: "IX_Connection_FriendbId",
                table: "Connections",
                newName: "IX_Connections_FriendbId");

            migrationBuilder.RenameIndex(
                name: "IX_Connection_FriendaId",
                table: "Connections",
                newName: "IX_Connections_FriendaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connections",
                table: "Connections",
                column: "ConnectionId");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WebSet = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_FriendaId",
                table: "Connections",
                column: "FriendaId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Users_FriendbId",
                table: "Connections",
                column: "FriendbId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_FriendaId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Users_FriendbId",
                table: "Connections");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Connections",
                table: "Connections");

            migrationBuilder.RenameTable(
                name: "Connections",
                newName: "Connection");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_FriendbId",
                table: "Connection",
                newName: "IX_Connection_FriendbId");

            migrationBuilder.RenameIndex(
                name: "IX_Connections_FriendaId",
                table: "Connection",
                newName: "IX_Connection_FriendaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Connection",
                table: "Connection",
                column: "ConnectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_Users_FriendaId",
                table: "Connection",
                column: "FriendaId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Connection_Users_FriendbId",
                table: "Connection",
                column: "FriendbId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
