using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YS.Azure.ToDo.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskFilesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskFiles_ArchivedTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "ArchivedTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskFiles_TaskId",
                table: "TaskFiles",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskFiles");
        }
    }
}
