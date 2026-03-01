using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class model_builder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommand_Command_CommandId",
                table: "SaltCommand");

            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommand_SaltTasks_SaltTaskId",
                table: "SaltCommand");

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommand_Command_CommandId",
                table: "SaltCommand",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommand_SaltTasks_SaltTaskId",
                table: "SaltCommand",
                column: "SaltTaskId",
                principalTable: "SaltTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommand_Command_CommandId",
                table: "SaltCommand");

            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommand_SaltTasks_SaltTaskId",
                table: "SaltCommand");

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommand_Command_CommandId",
                table: "SaltCommand",
                column: "CommandId",
                principalTable: "Command",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommand_SaltTasks_SaltTaskId",
                table: "SaltCommand",
                column: "SaltTaskId",
                principalTable: "SaltTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
