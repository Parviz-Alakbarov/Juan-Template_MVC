using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class ShoesCategoryIdMadeNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Shoes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Shoes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Categories_CategoryId",
                table: "Shoes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
