using Microsoft.EntityFrameworkCore.Migrations;

namespace logPropertis.Migrations
{
    public partial class Update2Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RowId",
                table: "ChangeLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowId",
                table: "ChangeLogs");
        }
    }
}
