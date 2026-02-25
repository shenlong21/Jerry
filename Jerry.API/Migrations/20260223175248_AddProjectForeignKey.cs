using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Project",
                table: "Users",
                column: "Project");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Projects_Project",
                table: "Users",
                column: "Project",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_Project",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Project",
                table: "Users");
        }
    }
}
