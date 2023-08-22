using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HowTo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addmoderation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "CourseContext",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "CourseContext",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ArticleContext",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CourseContext_AuthorId",
                table: "CourseContext",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContext_ContributorEntityContext_AuthorId",
                table: "CourseContext",
                column: "AuthorId",
                principalTable: "ContributorEntityContext",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContext_ContributorEntityContext_AuthorId",
                table: "CourseContext");

            migrationBuilder.DropIndex(
                name: "IX_CourseContext_AuthorId",
                table: "CourseContext");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "CourseContext");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseContext");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ArticleContext");
        }
    }
}
