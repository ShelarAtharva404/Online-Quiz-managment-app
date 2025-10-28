using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedByProfessorIdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedByProfessorId",
                table: "Users",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByProfessorId",
                table: "Users");
        }
    }
}
