using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehourse.ProductPlacement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialW : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WarehouseManagement");

            migrationBuilder.CreateTable(
                name: "pending_products",
                schema: "WarehouseManagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    description = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    height = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    length = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    width = table.Column<double>(type: "double precision", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pending_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_storages",
                schema: "WarehouseManagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_id = table.Column<Guid>(type: "uuid", nullable: false),
                    section_row_number = table.Column<int>(type: "integer", nullable: false),
                    shelf_column_number = table.Column<int>(type: "integer", nullable: false),
                    shelf_row_number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_storages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "WarehouseManagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    category = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    description = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    height = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    length = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    width = table.Column<double>(type: "double precision", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pending_products",
                schema: "WarehouseManagement");

            migrationBuilder.DropTable(
                name: "product_storages",
                schema: "WarehouseManagement");

            migrationBuilder.DropTable(
                name: "products",
                schema: "WarehouseManagement");
        }
    }
}
