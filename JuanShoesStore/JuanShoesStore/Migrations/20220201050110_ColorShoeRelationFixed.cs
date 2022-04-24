using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class ColorShoeRelationFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ColorsItem",
                table: "ColorsItem");

            migrationBuilder.RenameTable(
                name: "ColorsItem",
                newName: "ShoeColorItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoeColorItems",
                table: "ShoeColorItems",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoeColorItems",
                table: "ShoeColorItems");

            migrationBuilder.RenameTable(
                name: "ShoeColorItems",
                newName: "ColorsItem");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ColorsItem",
                table: "ColorsItem",
                column: "Id");
        }
    }
}
