using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HowTo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixlastinteractivedto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LastWritingOfAnswerContext",
                table: "LastWritingOfAnswerContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastProgramWritingContext",
                table: "LastProgramWritingContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastChoiceOfAnswerContext",
                table: "LastChoiceOfAnswerContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastCheckListContext",
                table: "LastCheckListContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InteractiveContext",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "AnswersJsonBoolArray",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "ClausesJsonStringArray",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "Output",
                table: "InteractiveContext");

            migrationBuilder.DropColumn(
                name: "QuestionsJsonStringArray",
                table: "InteractiveContext");

            migrationBuilder.RenameTable(
                name: "InteractiveContext",
                newName: "WritingOfAnswerContext");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LastWritingOfAnswerContext",
                newName: "InteractiveId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LastProgramWritingContext",
                newName: "InteractiveId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LastChoiceOfAnswerContext",
                newName: "InteractiveId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LastCheckListContext",
                newName: "InteractiveId");

            migrationBuilder.AlterColumn<int>(
                name: "InteractiveId",
                table: "LastWritingOfAnswerContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "InteractiveId",
                table: "LastProgramWritingContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "InteractiveId",
                table: "LastChoiceOfAnswerContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "InteractiveId",
                table: "LastCheckListContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "WritingOfAnswerContext",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastWritingOfAnswerContext",
                table: "LastWritingOfAnswerContext",
                columns: new[] { "InteractiveId", "CourseId", "ArticleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastProgramWritingContext",
                table: "LastProgramWritingContext",
                columns: new[] { "InteractiveId", "CourseId", "ArticleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastChoiceOfAnswerContext",
                table: "LastChoiceOfAnswerContext",
                columns: new[] { "InteractiveId", "CourseId", "ArticleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastCheckListContext",
                table: "LastCheckListContext",
                columns: new[] { "InteractiveId", "CourseId", "ArticleId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_WritingOfAnswerContext",
                table: "WritingOfAnswerContext",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CheckListContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClausesJsonStringArray = table.Column<string>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceOfAnswerContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionsJsonStringArray = table.Column<string>(type: "TEXT", nullable: false),
                    AnswersJsonBoolArray = table.Column<string>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceOfAnswerContext", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramWritingContext",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Output = table.Column<string>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    ArticleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramWritingContext", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckListContext");

            migrationBuilder.DropTable(
                name: "ChoiceOfAnswerContext");

            migrationBuilder.DropTable(
                name: "ProgramWritingContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastWritingOfAnswerContext",
                table: "LastWritingOfAnswerContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastProgramWritingContext",
                table: "LastProgramWritingContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastChoiceOfAnswerContext",
                table: "LastChoiceOfAnswerContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LastCheckListContext",
                table: "LastCheckListContext");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WritingOfAnswerContext",
                table: "WritingOfAnswerContext");

            migrationBuilder.RenameTable(
                name: "WritingOfAnswerContext",
                newName: "InteractiveContext");

            migrationBuilder.RenameColumn(
                name: "InteractiveId",
                table: "LastWritingOfAnswerContext",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InteractiveId",
                table: "LastProgramWritingContext",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InteractiveId",
                table: "LastChoiceOfAnswerContext",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InteractiveId",
                table: "LastCheckListContext",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LastWritingOfAnswerContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LastProgramWritingContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LastChoiceOfAnswerContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "LastCheckListContext",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AnswersJsonBoolArray",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClausesJsonStringArray",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionsJsonStringArray",
                table: "InteractiveContext",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastWritingOfAnswerContext",
                table: "LastWritingOfAnswerContext",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastProgramWritingContext",
                table: "LastProgramWritingContext",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastChoiceOfAnswerContext",
                table: "LastChoiceOfAnswerContext",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LastCheckListContext",
                table: "LastCheckListContext",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InteractiveContext",
                table: "InteractiveContext",
                column: "Id");
        }
    }
}
