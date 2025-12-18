using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triumph.SMS.Remote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModificationOfStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceivedBy",
                table: "RecentPayment",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedBy",
                table: "RecentPayment");
        }
    }
}
