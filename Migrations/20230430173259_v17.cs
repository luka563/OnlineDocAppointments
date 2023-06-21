using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "comments",
                newName: "CommentContent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommentContent",
                table: "comments",
                newName: "Text");
        }
    }
}
