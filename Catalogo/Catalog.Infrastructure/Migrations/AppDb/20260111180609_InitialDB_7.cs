using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class InitialDB_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserAt",
                schema: "Dbo",
                table: "Services",
                type: "varchar(80)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "UserAt",
                schema: "Dbo",
                table: "Categories",
                type: "varchar(80)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserAt",
                schema: "Dbo",
                table: "Services",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(80)");

            migrationBuilder.AlterColumn<string>(
                name: "UserAt",
                schema: "Dbo",
                table: "Categories",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(80)");
        }
    }
}
