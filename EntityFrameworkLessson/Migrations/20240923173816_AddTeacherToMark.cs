using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkLessson.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherToMark : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "Marks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_TeacherId",
                table: "Marks",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Person_TeacherId",
                table: "Marks",
                column: "TeacherId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Person_TeacherId",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_TeacherId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Marks");
        }
    }
}
