using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class CategoryRelationCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Shoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_CategoryId",
                table: "Shoes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_CategoryId",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Shoes");
        }
    }
}
