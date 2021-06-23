using Microsoft.EntityFrameworkCore.Migrations;

namespace DevOPS_Project.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "ToolUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToolUser",
                table: "ToolUser",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToolUser",
                table: "ToolUser");

            migrationBuilder.RenameTable(
                name: "ToolUser",
                newName: "User");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserID");
        }
    }
}
