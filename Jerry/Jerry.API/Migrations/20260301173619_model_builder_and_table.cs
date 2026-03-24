using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class model_builder_and_table : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaltCommand",
                table: "SaltCommand");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Command",
                table: "Command");

            migrationBuilder.RenameTable(
                name: "SaltCommand",
                newName: "SaltCommands");

            migrationBuilder.RenameTable(
                name: "Command",
                newName: "Commands");

            migrationBuilder.RenameIndex(
                name: "IX_SaltCommand_SaltTaskId",
                table: "SaltCommands",
                newName: "IX_SaltCommands_SaltTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_SaltCommand_CommandId",
                table: "SaltCommands",
                newName: "IX_SaltCommands_CommandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaltCommands",
                table: "SaltCommands",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commands",
                table: "Commands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommands_Commands_CommandId",
                table: "SaltCommands",
                column: "CommandId",
                principalTable: "Commands",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SaltCommands_SaltTasks_SaltTaskId",
                table: "SaltCommands",
                column: "SaltTaskId",
                principalTable: "SaltTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommands_Commands_CommandId",
                table: "SaltCommands");

            migrationBuilder.DropForeignKey(
                name: "FK_SaltCommands_SaltTasks_SaltTaskId",
                table: "SaltCommands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SaltCommands",
                table: "SaltCommands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commands",
                table: "Commands");

            migrationBuilder.RenameTable(
                name: "SaltCommands",
                newName: "SaltCommand");

            migrationBuilder.RenameTable(
                name: "Commands",
                newName: "Command");

            migrationBuilder.RenameIndex(
                name: "IX_SaltCommands_SaltTaskId",
                table: "SaltCommand",
                newName: "IX_SaltCommand_SaltTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_SaltCommands_CommandId",
                table: "SaltCommand",
                newName: "IX_SaltCommand_CommandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaltCommand",
                table: "SaltCommand",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Command",
                table: "Command",
                column: "Id");

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
    }
}
