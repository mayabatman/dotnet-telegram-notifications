using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProteiTelegramBot.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TelegramLogin = table.Column<string>(type: "text", nullable: false),
                    ProteiLogin = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DutyInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    DutyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DutyInformation_Employees_Id",
                        column: x => x.Id,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DutyInformation_DutyDate",
                table: "DutyInformation",
                column: "DutyDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ProteiLogin",
                table: "Employees",
                column: "ProteiLogin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_TelegramLogin",
                table: "Employees",
                column: "TelegramLogin",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DutyInformation");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
