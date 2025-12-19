using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Triumph.SMS.Remote.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedSchoolConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchoolInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Logo = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 225, nullable: true),
                    Website = table.Column<string>(type: "TEXT", maxLength: 225, nullable: true),
                    Motto = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lisence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LisenceKey = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    IssuedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SchoolId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lisence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lisence_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lisence_SchoolId",
                table: "Lisence",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lisence");

            migrationBuilder.DropTable(
                name: "SchoolInformation");
        }
    }
}
