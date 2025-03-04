using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilteredOrderEntityOrderProductEntity");

            migrationBuilder.DropTable(
                name: "FilteredOrderEntityPersonEntity");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "FilteredOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryTime",
                table: "FilteredOrders");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "FilteredOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "FilteredOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryTime",
                table: "FilteredOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "PersonId",
                table: "FilteredOrders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateTable(
                name: "FilteredOrderEntityPersonEntity",
                columns: table => new
                {
                    FilteredOrderEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilteredOrderEntityPersonEntity", x => new { x.FilteredOrderEntityId, x.PersonsId });
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityPersonEntity_FilteredOrders_FilteredOrde~",
                        column: x => x.FilteredOrderEntityId,
                        principalTable: "FilteredOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilteredOrderEntityPersonEntity_Persons_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOrderEntityOrderProductEntity_OrderProductsOrderId_~",
                table: "FilteredOrderEntityOrderProductEntity",
                columns: new[] { "OrderProductsOrderId", "OrderProductsProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_FilteredOrderEntityPersonEntity_PersonsId",
                table: "FilteredOrderEntityPersonEntity",
                column: "PersonsId");
        }
    }
}
