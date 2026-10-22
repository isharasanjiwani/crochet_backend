using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrochetWorldApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNaame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceDepends",
                table: "products",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "products",
                newName: "PriceDepends");
        }
    }
}
