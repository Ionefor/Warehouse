using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.WarehouseManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class W : Migration
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
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    notification_email = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    height = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    length = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    width = table.Column<double>(type: "double precision", maxLength: 40, nullable: false)
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
                    type = table.Column<string>(type: "text", nullable: false),
                    section_rows = table.Column<string>(type: "jsonb", nullable: true),
                    warehouse_id = table.Column<Guid>(type: "uuid", nullable: false),
                    height = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    length = table.Column<double>(type: "double precision", maxLength: 40, nullable: false),
                    width = table.Column<double>(type: "double precision", maxLength: 40, nullable: false)
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
