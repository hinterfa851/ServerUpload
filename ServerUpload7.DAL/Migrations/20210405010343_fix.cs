using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerUpload7.DAL.Migrations
{
    public partial class fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Version_Materials_MaterialId",
                table: "Version");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Version",
                table: "Version");

            migrationBuilder.RenameTable(
                name: "Version",
                newName: "Versions");

            migrationBuilder.RenameIndex(
                name: "IX_Version_MaterialId",
                table: "Versions",
                newName: "IX_Versions_MaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Versions",
                table: "Versions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Versions_Materials_MaterialId",
                table: "Versions",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Versions_Materials_MaterialId",
                table: "Versions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Versions",
                table: "Versions");

            migrationBuilder.RenameTable(
                name: "Versions",
                newName: "Version");

            migrationBuilder.RenameIndex(
                name: "IX_Versions_MaterialId",
                table: "Version",
                newName: "IX_Version_MaterialId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Version",
                table: "Version",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Version_Materials_MaterialId",
                table: "Version",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
