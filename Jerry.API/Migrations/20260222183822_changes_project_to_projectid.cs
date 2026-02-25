using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jerry.API.Migrations
{
    /// <inheritdoc />
    public partial class changes_project_to_projectid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Project",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "randomblob(16)",
                oldClrType: typeof(Guid),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Project",
                table: "Users",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldDefaultValueSql: "randomblob(16)");
        }
    }
}
