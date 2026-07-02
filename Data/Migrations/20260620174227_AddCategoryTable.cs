using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TO_DO_List.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "TaskToDo");

            migrationBuilder.RenameColumn(
                name: "Categories",
                table: "TaskRepetitives",
                newName: "UserID");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "TaskToDo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCategories",
                table: "TaskToDo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "TaskRepetitives",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdCategories",
                table: "TaskRepetitives",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskToDo_CategoryId",
                table: "TaskToDo",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskRepetitives_CategoryId",
                table: "TaskRepetitives",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRepetitives_Categories_CategoryId",
                table: "TaskRepetitives",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskToDo_Categories_CategoryId",
                table: "TaskToDo",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskRepetitives_Categories_CategoryId",
                table: "TaskRepetitives");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskToDo_Categories_CategoryId",
                table: "TaskToDo");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_TaskToDo_CategoryId",
                table: "TaskToDo");

            migrationBuilder.DropIndex(
                name: "IX_TaskRepetitives_CategoryId",
                table: "TaskRepetitives");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TaskToDo");

            migrationBuilder.DropColumn(
                name: "IdCategories",
                table: "TaskToDo");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TaskRepetitives");

            migrationBuilder.DropColumn(
                name: "IdCategories",
                table: "TaskRepetitives");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "TaskRepetitives",
                newName: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "TaskToDo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
