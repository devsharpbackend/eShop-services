﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.API.Migrations
{
    public partial class initialdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.EnsureSchema(
                name: "Supplier");

            migrationBuilder.CreateSequence(
                name: "catalog_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "catalog_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "supplier_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "CatalogType",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Catalog",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceWithDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDiscount = table.Column<bool>(type: "bit", nullable: false),
                    PictureFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatalogTypeId = table.Column<int>(type: "int", nullable: false),
                    AvailableStock = table.Column<int>(type: "int", nullable: false),
                    StockThreshold = table.Column<int>(type: "int", nullable: false),
                    MaxStockThreshold = table.Column<int>(type: "int", nullable: false),
                    _catalogTypeId = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogType__catalogTypeId",
                        column: x => x._catalogTypeId,
                        principalSchema: "Catalog",
                        principalTable: "CatalogType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Catalog_CatalogType_CatalogTypeId",
                        column: x => x.CatalogTypeId,
                        principalSchema: "Catalog",
                        principalTable: "CatalogType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "Supplier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatalogTypeId = table.Column<int>(type: "int", nullable: false),
                    _catalogTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplier_CatalogType__catalogTypeId",
                        column: x => x._catalogTypeId,
                        principalSchema: "Catalog",
                        principalTable: "CatalogType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SupplierItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierItem_Supplier_SupplierId1",
                        column: x => x.SupplierId1,
                        principalSchema: "Supplier",
                        principalTable: "Supplier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Catalog__catalogTypeId",
                schema: "Catalog",
                table: "Catalog",
                column: "_catalogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalog_CatalogTypeId",
                schema: "Catalog",
                table: "Catalog",
                column: "CatalogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier__catalogTypeId",
                schema: "Supplier",
                table: "Supplier",
                column: "_catalogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierItem_SupplierId1",
                table: "SupplierItem",
                column: "SupplierId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catalog",
                schema: "Catalog");

            migrationBuilder.DropTable(
                name: "SupplierItem");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "Supplier");

            migrationBuilder.DropTable(
                name: "CatalogType",
                schema: "Catalog");

            migrationBuilder.DropSequence(
                name: "catalog_hilo");

            migrationBuilder.DropSequence(
                name: "catalog_type_hilo");

            migrationBuilder.DropSequence(
                name: "supplier_hilo");
        }
    }
}