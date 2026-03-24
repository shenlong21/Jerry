using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class Added_commands_and_SaltCommand_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Command = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaltCommands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SaltTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    CommandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CommandsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaltCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaltCommands_Commands_CommandsId",
                        column: x => x.CommandsId,
                        principalTable: "Commands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaltCommands_SaltTasks_SaltTaskId",
                        column: x => x.SaltTaskId,
                        principalTable: "SaltTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaltCommands_CommandsId",
                table: "SaltCommands",
                column: "CommandsId");

            migrationBuilder.CreateIndex(
                name: "IX_SaltCommands_SaltTaskId",
                table: "SaltCommands",
                column: "SaltTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaltCommands");

            migrationBuilder.DropTable(
                name: "Commands");
        }
    }
}
