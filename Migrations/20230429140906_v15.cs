using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromWhoID",
                table: "ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToWhoID",
                table: "ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ratings_FromWhoID",
                table: "ratings",
                column: "FromWhoID");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_ToWhoID",
                table: "ratings",
                column: "ToWhoID");

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_users_FromWhoID",
                table: "ratings",
                column: "FromWhoID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ratings_users_ToWhoID",
                table: "ratings",
                column: "ToWhoID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ratings_users_FromWhoID",
                table: "ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_ratings_users_ToWhoID",
                table: "ratings");

            migrationBuilder.DropIndex(
                name: "IX_ratings_FromWhoID",
                table: "ratings");

            migrationBuilder.DropIndex(
                name: "IX_ratings_ToWhoID",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "FromWhoID",
                table: "ratings");

            migrationBuilder.DropColumn(
                name: "ToWhoID",
                table: "ratings");
        }
    }
}
