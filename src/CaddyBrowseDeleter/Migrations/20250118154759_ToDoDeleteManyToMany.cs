using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CaddyBrowseDeleter.Migrations
{
    /// <inheritdoc />
    public partial class ToDoDeleteManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoDeleteFiles_Users_UserId",
                table: "ToDoDeleteFiles");

            migrationBuilder.DropIndex(
                name: "IX_ToDoDeleteFiles_UserId",
                table: "ToDoDeleteFiles");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ToDoDeleteFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "ToDoDeleteFiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DirPath",
                table: "ToDoDeleteFiles",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ToDoDeleteFileUser",
                columns: table => new
                {
                    ToDoDeleteFilesId = table.Column<long>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoDeleteFileUser", x => new { x.ToDoDeleteFilesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ToDoDeleteFileUser_ToDoDeleteFiles_ToDoDeleteFilesId",
                        column: x => x.ToDoDeleteFilesId,
                        principalTable: "ToDoDeleteFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ToDoDeleteFileUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoDeleteFileUser_UsersId",
                table: "ToDoDeleteFileUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoDeleteFileUser");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "FilePath",
                table: "ToDoDeleteFiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "DirPath",
                table: "ToDoDeleteFiles",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ToDoDeleteFiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoDeleteFiles_UserId",
                table: "ToDoDeleteFiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoDeleteFiles_Users_UserId",
                table: "ToDoDeleteFiles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
