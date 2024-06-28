using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCBEM.Migrations
{
    /// <inheritdoc />
    public partial class ver1002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorRole",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorRole",
                table: "Papers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorresponding",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorRole",
                table: "Papers");

            migrationBuilder.DropColumn(
                name: "IsCorresponding",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "AuthorRole",
                table: "Authors",
                type: "int",
                nullable: true);
        }
    }
}
