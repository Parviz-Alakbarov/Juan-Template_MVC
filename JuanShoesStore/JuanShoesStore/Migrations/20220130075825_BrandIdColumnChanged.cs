using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class BrandIdColumnChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Brands_BrandId1",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_BrandId1",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "BrandId1",
                table: "Shoes");

            migrationBuilder.AlterColumn<int>(
                name: "BrandId",
                table: "Shoes",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_BrandId",
                table: "Shoes",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Brands_BrandId",
                table: "Shoes",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shoes_Brands_BrandId",
                table: "Shoes");

            migrationBuilder.DropIndex(
                name: "IX_Shoes_BrandId",
                table: "Shoes");

            migrationBuilder.AlterColumn<string>(
                name: "BrandId",
                table: "Shoes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BrandId1",
                table: "Shoes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shoes_BrandId1",
                table: "Shoes",
                column: "BrandId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Shoes_Brands_BrandId1",
                table: "Shoes",
                column: "BrandId1",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
