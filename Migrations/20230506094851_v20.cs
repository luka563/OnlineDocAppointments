using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermInt_interventions_InterventionID",
                table: "TermInt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_interventions",
                table: "interventions");

            migrationBuilder.RenameTable(
                name: "interventions",
                newName: "Intervention");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Intervention",
                table: "Intervention",
                column: "InterventionID");

            migrationBuilder.AddForeignKey(
                name: "FK_TermInt_Intervention_InterventionID",
                table: "TermInt",
                column: "InterventionID",
                principalTable: "Intervention",
                principalColumn: "InterventionID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TermInt_Intervention_InterventionID",
                table: "TermInt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Intervention",
                table: "Intervention");

            migrationBuilder.RenameTable(
                name: "Intervention",
                newName: "interventions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interventions",
                table: "interventions",
                column: "InterventionID");

            migrationBuilder.AddForeignKey(
                name: "FK_TermInt_interventions_InterventionID",
                table: "TermInt",
                column: "InterventionID",
                principalTable: "interventions",
                principalColumn: "InterventionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
