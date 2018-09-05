using Microsoft.EntityFrameworkCore.Migrations;

namespace logPropertis.Migrations
{
    public partial class UpdateInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "ChangeLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "ChangeLogs");
        }
    }
}
