using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triumph.SMS.Remote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedStudentMiddleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherNames",
                table: "Students",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherNames",
                table: "Students");
        }
    }
}
