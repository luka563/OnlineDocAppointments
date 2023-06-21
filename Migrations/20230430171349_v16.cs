using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StomatoloskaOrdinacija.Migrations
{
    /// <inheritdoc />
    public partial class v16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToWho",
                table: "comments",
                newName: "CommentToID");

            migrationBuilder.RenameColumn(
                name: "FromWho",
                table: "comments",
                newName: "CommentFromID");

            migrationBuilder.CreateIndex(
                name: "IX_comments_CommentFromID",
                table: "comments",
                column: "CommentFromID");

            migrationBuilder.CreateIndex(
                name: "IX_comments_CommentToID",
                table: "comments",
                column: "CommentToID");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_CommentFromID",
                table: "comments",
                column: "CommentFromID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_users_CommentToID",
                table: "comments",
                column: "CommentToID",
                principalTable: "users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_CommentFromID",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_users_CommentToID",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_CommentFromID",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_CommentToID",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "CommentToID",
                table: "comments",
                newName: "ToWho");

            migrationBuilder.RenameColumn(
                name: "CommentFromID",
                table: "comments",
                newName: "FromWho");
        }
    }
}
