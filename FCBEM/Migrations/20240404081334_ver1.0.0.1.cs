using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCBEM.Migrations
{
    /// <inheritdoc />
    public partial class ver1001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaperId",
                table: "Papers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaperId",
                table: "Papers");
        }
    }
}
