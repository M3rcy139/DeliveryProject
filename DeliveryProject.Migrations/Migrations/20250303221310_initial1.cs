using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilteredOrderEntityProductEntity");

            migrationBuilder.DropTable(
                name: "OrderEntityProductEntity");

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_OrderProducts_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilteredOrderEntityOrderProductEntity",
                columns: table => new
                {
                    FilteredOrderEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderProductsOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderProductsProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilteredOrderEntityOrderProductEntity", x => new { x.FilteredOrderEntityId, x.OrderProductsOrderId, x.OrderProductsProductId });
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityOrderProductEntity_FilteredOrders_Filter~",
                        column: x => x.FilteredOrderEntityId,
                        principalTable: "FilteredOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityOrderProductEntity_OrderProducts_OrderPr~",
                        columns: x => new { x.OrderProductsOrderId, x.OrderProductsProductId },
                        principalTable: "OrderProducts",
                        principalColumns: new[] { "OrderId", "ProductId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOrderEntityOrderProductEntity_OrderProductsOrderId_~",
                table: "FilteredOrderEntityOrderProductEntity",
                columns: new[] { "OrderProductsOrderId", "OrderProductsProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilteredOrderEntityOrderProductEntity");

            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.CreateTable(
                name: "FilteredOrderEntityProductEntity",
                columns: table => new
                {
                    FilteredOrderEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilteredOrderEntityProductEntity", x => new { x.FilteredOrderEntityId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityProductEntity_FilteredOrders_FilteredOrd~",
                        column: x => x.FilteredOrderEntityId,
                        principalTable: "FilteredOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityProductEntity_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderEntityProductEntity",
                columns: table => new
                {
                    OrderEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntityProductEntity", x => new { x.OrderEntityId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_OrderEntityProductEntity_Orders_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderEntityProductEntity_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOrderEntityProductEntity_ProductsId",
                table: "FilteredOrderEntityProductEntity",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntityProductEntity_ProductsId",
                table: "OrderEntityProductEntity",
                column: "ProductsId");
        }
    }
}
