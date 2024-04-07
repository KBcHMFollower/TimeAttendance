using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeAttendanceApp.Migrations
{
    /// <inheritdoc />
    public partial class addFileRes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "TaskComments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "TaskComments");
        }
    }
}
