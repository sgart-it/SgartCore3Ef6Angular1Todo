using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SgartCore3Ef6Angular1Todo.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Color = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MyTasks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false),
                    Note = table.Column<string>(nullable: true),
                    Completed = table.Column<DateTime>(nullable: true),
                    CategoryID = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MyTasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MyTasks_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "ID", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "#BCBCBC", "Undefined" },
                    { 2, "#EB8D90", "Red" },
                    { 3, "#A6DD9E", "Green" },
                    { 4, "#97B3E4", "Blue" },
                    { 5, "#FFFA87", "Yellow" },
                    { 6, "#B09CDD", "Purple" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MyTasks_CategoryID",
                table: "MyTasks",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MyTasks");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
