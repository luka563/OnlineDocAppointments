using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Intervention",
                columns: table => new
                {
                    InterventionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InterventionTimeMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intervention", x => x.InterventionID);
                });

            migrationBuilder.CreateTable(
                name: "WorkingDay",
                columns: table => new
                {
                    WorkingDayID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorUserID = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDay", x => x.WorkingDayID);
                    table.ForeignKey(
                        name: "FK_WorkingDay_users_DoctorUserID",
                        column: x => x.DoctorUserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Term",
                columns: table => new
                {
                    TermID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    TermStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TermEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkingDayID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Term", x => x.TermID);
                    table.ForeignKey(
                        name: "FK_Term_WorkingDay_WorkingDayID",
                        column: x => x.WorkingDayID,
                        principalTable: "WorkingDay",
                        principalColumn: "WorkingDayID");
                    table.ForeignKey(
                        name: "FK_Term_users_UserID",
                        column: x => x.UserID,
                        principalTable: "users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TermInt",
                columns: table => new
                {
                    TermIntID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TermID = table.Column<int>(type: "int", nullable: false),
                    InterventionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermInt", x => x.TermIntID);
                    table.ForeignKey(
                        name: "FK_TermInt_Intervention_InterventionID",
                        column: x => x.InterventionID,
                        principalTable: "Intervention",
                        principalColumn: "InterventionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TermInt_Term_TermID",
                        column: x => x.TermID,
                        principalTable: "Term",
                        principalColumn: "TermID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Term_UserID",
                table: "Term",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Term_WorkingDayID",
                table: "Term",
                column: "WorkingDayID");

            migrationBuilder.CreateIndex(
                name: "IX_TermInt_InterventionID",
                table: "TermInt",
                column: "InterventionID");

            migrationBuilder.CreateIndex(
                name: "IX_TermInt_TermID",
                table: "TermInt",
                column: "TermID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDay_DoctorUserID",
                table: "WorkingDay",
                column: "DoctorUserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TermInt");

            migrationBuilder.DropTable(
                name: "Intervention");

            migrationBuilder.DropTable(
                name: "Term");

            migrationBuilder.DropTable(
                name: "WorkingDay");
        }
    }
}
