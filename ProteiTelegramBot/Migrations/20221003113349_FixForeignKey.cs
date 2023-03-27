using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProteiTelegramBot.Migrations
{
    public partial class FixForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DutyInformation_Employees_Id",
                table: "DutyInformation");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DutyInformation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "DutyEmployeeId",
                table: "DutyInformation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE \"DutyInformation\" SET \"DutyEmployeeId\" = \"Id\";");

            migrationBuilder.CreateIndex(
                name: "IX_DutyInformation_DutyEmployeeId",
                table: "DutyInformation",
                column: "DutyEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DutyInformation_Employees_DutyEmployeeId",
                table: "DutyInformation",
                column: "DutyEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DutyInformation_Employees_DutyEmployeeId",
                table: "DutyInformation");

            migrationBuilder.DropIndex(
                name: "IX_DutyInformation_DutyEmployeeId",
                table: "DutyInformation");

            migrationBuilder.DropColumn(
                name: "DutyEmployeeId",
                table: "DutyInformation");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DutyInformation",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_DutyInformation_Employees_Id",
                table: "DutyInformation",
                column: "Id",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
