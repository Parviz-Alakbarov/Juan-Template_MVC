using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class ColorShoeRelationCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShoeColorItems_ColorId",
                table: "ShoeColorItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoeColorItems_ShoeId",
                table: "ShoeColorItems",
                column: "ShoeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoeColorItems_Colors_ColorId",
                table: "ShoeColorItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoeColorItems_Shoes_ShoeId",
                table: "ShoeColorItems",
                column: "ShoeId",
                principalTable: "Shoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoeColorItems_Colors_ColorId",
                table: "ShoeColorItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoeColorItems_Shoes_ShoeId",
                table: "ShoeColorItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoeColorItems_ColorId",
                table: "ShoeColorItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoeColorItems_ShoeId",
                table: "ShoeColorItems");
        }
    }
}
