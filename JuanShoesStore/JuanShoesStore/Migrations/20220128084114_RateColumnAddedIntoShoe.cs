using Microsoft.EntityFrameworkCore.Migrations;

namespace JuanShoesStore.Migrations
{
    public partial class RateColumnAddedIntoShoe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dest",
                table: "Shoes");

            migrationBuilder.AddColumn<string>(
                name: "Desc",
                table: "Shoes",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Shoes",
                type: "decimal(2,1)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desc",
                table: "Shoes");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Shoes");

            migrationBuilder.AddColumn<string>(
                name: "Dest",
                table: "Shoes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
