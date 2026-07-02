using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TO_DO_List.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskRepetitivetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "TaskToDo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TaskRepetitives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoTillDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CereatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRepetitive = table.Column<bool>(type: "bit", nullable: false),
                    HowOften = table.Column<int>(type: "int", nullable: false),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskRepetitives", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskRepetitives");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "TaskToDo");
        }
    }
}
