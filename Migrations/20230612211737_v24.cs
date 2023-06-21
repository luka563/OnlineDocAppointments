using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterventionID",
                table: "uss",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpecInt",
                columns: table => new
                {
                    SpecIntID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecInt", x => x.SpecIntID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_uss_InterventionID",
                table: "uss",
                column: "InterventionID");

            migrationBuilder.AddForeignKey(
                name: "FK_uss_Intervention_InterventionID",
                table: "uss",
                column: "InterventionID",
                principalTable: "Intervention",
                principalColumn: "InterventionID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_uss_Intervention_InterventionID",
                table: "uss");

            migrationBuilder.DropTable(
                name: "SpecInt");

            migrationBuilder.DropIndex(
                name: "IX_uss_InterventionID",
                table: "uss");

            migrationBuilder.DropColumn(
                name: "InterventionID",
                table: "uss");
        }
    }
}
