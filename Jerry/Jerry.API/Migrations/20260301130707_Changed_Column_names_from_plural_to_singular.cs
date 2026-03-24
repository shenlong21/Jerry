using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Column_names_from_plural_to_singular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaltCommands");

            migrationBuilder.DropTable(
                name: "Commands");

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CommandString = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaltCommand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SaltTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    CommandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaltCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaltCommand_Command_CommandId",
                        column: x => x.CommandId,
                        principalTable: "Command",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaltCommand_SaltTasks_SaltTaskId",
                        column: x => x.SaltTaskId,
                        principalTable: "SaltTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SaltCommand_CommandId",
                table: "SaltCommand",
                column: "CommandId");

            migrationBuilder.CreateIndex(
                name: "IX_SaltCommand_SaltTaskId",
                table: "SaltCommand",
                column: "SaltTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SaltCommand");

            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.CreateTable(
                name: "Commands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Command = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
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
                    CommandsId = table.Column<int>(type: "INTEGER", nullable: true),
                    SaltTaskId = table.Column<int>(type: "INTEGER", nullable: false),
                    CommandId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
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
    }
}
