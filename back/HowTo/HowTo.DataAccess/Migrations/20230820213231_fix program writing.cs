using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HowTo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixprogramwriting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Output",
                table: "ProgramWritingContext");

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "LastProgramWritingContext",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Output",
                table: "LastProgramWritingContext");

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "ProgramWritingContext",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
