using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InterventionID",
                table: "SpecInt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "USID",
                table: "SpecInt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SI",
                columns: table => new
                {
                    SIID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecId = table.Column<int>(type: "int", nullable: false),
                    IntId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SI", x => x.SIID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecInt_InterventionID",
                table: "SpecInt",
                column: "InterventionID");

            migrationBuilder.CreateIndex(
                name: "IX_SpecInt_USID",
                table: "SpecInt",
                column: "USID");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecInt_Intervention_InterventionID",
                table: "SpecInt",
                column: "InterventionID",
                principalTable: "Intervention",
                principalColumn: "InterventionID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SpecInt_uss_USID",
                table: "SpecInt",
                column: "USID",
                principalTable: "uss",
                principalColumn: "USID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecInt_Intervention_InterventionID",
                table: "SpecInt");

            migrationBuilder.DropForeignKey(
                name: "FK_SpecInt_uss_USID",
                table: "SpecInt");

            migrationBuilder.DropTable(
                name: "SI");

            migrationBuilder.DropIndex(
                name: "IX_SpecInt_InterventionID",
                table: "SpecInt");

            migrationBuilder.DropIndex(
                name: "IX_SpecInt_USID",
                table: "SpecInt");

            migrationBuilder.DropColumn(
                name: "InterventionID",
                table: "SpecInt");

            migrationBuilder.DropColumn(
                name: "USID",
                table: "SpecInt");
        }
    }
}
