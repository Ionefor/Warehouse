using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehourse.ProductPlacement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WWW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "width",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "size_width");

            migrationBuilder.RenameColumn(
                name: "length",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "size_length");

            migrationBuilder.RenameColumn(
                name: "height",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "size_height");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "size_width",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "width");

            migrationBuilder.RenameColumn(
                name: "size_length",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "length");

            migrationBuilder.RenameColumn(
                name: "size_height",
                schema: "WarehouseManagement",
                table: "pending_products",
                newName: "height");
        }
    }
}
