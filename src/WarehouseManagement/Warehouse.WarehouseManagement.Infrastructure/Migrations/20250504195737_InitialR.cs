using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.WarehouseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialR : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "WarehouseManagement");

            migrationBuilder.CreateTable(
                name: "warehouses",
                schema: "WarehouseManagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    size_height = table.Column<double>(type: "double precision", nullable: false),
                    size_length = table.Column<double>(type: "double precision", nullable: false),
                    size_width = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_warehouses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                schema: "WarehouseManagement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    section_rows = table.Column<string>(type: "text", nullable: false),
                    size_height = table.Column<double>(type: "double precision", nullable: false),
                    size_length = table.Column<double>(type: "double precision", nullable: false),
                    size_width = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sections", x => x.id);
                    table.ForeignKey(
                        name: "fk_sections_warehouses_warehouse_id",
                        column: x => x.warehouse_id,
                        principalSchema: "WarehouseManagement",
                        principalTable: "warehouses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sections_warehouse_id",
                schema: "WarehouseManagement",
                table: "sections",
                column: "warehouse_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sections",
                schema: "WarehouseManagement");

            migrationBuilder.DropTable(
                name: "warehouses",
                schema: "WarehouseManagement");
        }
    }
}
