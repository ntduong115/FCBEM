using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCBEM.Migrations
{
    /// <inheritdoc />
    public partial class ver1003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorRole",
                table: "Papers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorRole",
                table: "Papers",
                type: "int",
                nullable: true);
        }
    }
}
