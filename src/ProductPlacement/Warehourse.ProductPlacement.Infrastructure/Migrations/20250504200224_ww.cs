using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehourse.ProductPlacement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shelf_row_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "shelf_position_shelf_row_number");

            migrationBuilder.RenameColumn(
                name: "shelf_column_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "shelf_position_shelf_column_number");

            migrationBuilder.RenameColumn(
                name: "section_row_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "shelf_position_section_row_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "shelf_position_shelf_row_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "shelf_row_number");

            migrationBuilder.RenameColumn(
                name: "shelf_position_shelf_column_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "shelf_column_number");

            migrationBuilder.RenameColumn(
                name: "shelf_position_section_row_number",
                schema: "WarehouseManagement",
                table: "product_storages",
                newName: "section_row_number");
        }
    }
}
