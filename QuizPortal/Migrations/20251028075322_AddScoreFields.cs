using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddScoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TakenOn",
                table: "Scores",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "Marks",
                table: "Scores",
                newName: "TotalQuestions");

            migrationBuilder.AddColumn<int>(
                name: "CorrectAnswers",
                table: "Scores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                table: "Scores",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectAnswers",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Scores");

            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "Scores",
                newName: "Marks");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "Scores",
                newName: "TakenOn");
        }
    }
}
