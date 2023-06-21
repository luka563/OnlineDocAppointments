using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "uss");

            migrationBuilder.AddColumn<int>(
                name: "SpecializationUSID",
                table: "users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_SpecializationUSID",
                table: "users",
                column: "SpecializationUSID");

            migrationBuilder.AddForeignKey(
                name: "FK_users_uss_SpecializationUSID",
                table: "users",
                column: "SpecializationUSID",
                principalTable: "uss",
                principalColumn: "USID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_uss_SpecializationUSID",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_SpecializationUSID",
                table: "users");

            migrationBuilder.DropColumn(
                name: "SpecializationUSID",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "uss",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
