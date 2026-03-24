using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class SaltTask_Added_Script_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Script",
                table: "SaltTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Script",
                table: "SaltTasks");
        }
    }
}
