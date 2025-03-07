using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class initial4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempDeliverySlot_TempDeliveryPersons_DeliveryPersonId",
                table: "TempDeliverySlot");

            migrationBuilder.DropForeignKey(
                name: "FK_TempPersonContact_TempDeliveryPersons_DeliveryPersonId",
                table: "TempPersonContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempPersonContact",
                table: "TempPersonContact");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempDeliverySlot",
                table: "TempDeliverySlot");

            migrationBuilder.RenameTable(
                name: "TempPersonContact",
                newName: "TempPersonContacts");

            migrationBuilder.RenameTable(
                name: "TempDeliverySlot",
                newName: "TempDeliverySlots");

            migrationBuilder.RenameIndex(
                name: "IX_TempPersonContact_DeliveryPersonId",
                table: "TempPersonContacts",
                newName: "IX_TempPersonContacts_DeliveryPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_TempDeliverySlot_DeliveryPersonId",
                table: "TempDeliverySlots",
                newName: "IX_TempDeliverySlots_DeliveryPersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempPersonContacts",
                table: "TempPersonContacts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempDeliverySlots",
                table: "TempDeliverySlots",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TempDeliverySlots_TempDeliveryPersons_DeliveryPersonId",
                table: "TempDeliverySlots",
                column: "DeliveryPersonId",
                principalTable: "TempDeliveryPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TempPersonContacts_TempDeliveryPersons_DeliveryPersonId",
                table: "TempPersonContacts",
                column: "DeliveryPersonId",
                principalTable: "TempDeliveryPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TempDeliverySlots_TempDeliveryPersons_DeliveryPersonId",
                table: "TempDeliverySlots");

            migrationBuilder.DropForeignKey(
                name: "FK_TempPersonContacts_TempDeliveryPersons_DeliveryPersonId",
                table: "TempPersonContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempPersonContacts",
                table: "TempPersonContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempDeliverySlots",
                table: "TempDeliverySlots");

            migrationBuilder.RenameTable(
                name: "TempPersonContacts",
                newName: "TempPersonContact");

            migrationBuilder.RenameTable(
                name: "TempDeliverySlots",
                newName: "TempDeliverySlot");

            migrationBuilder.RenameIndex(
                name: "IX_TempPersonContacts_DeliveryPersonId",
                table: "TempPersonContact",
                newName: "IX_TempPersonContact_DeliveryPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_TempDeliverySlots_DeliveryPersonId",
                table: "TempDeliverySlot",
                newName: "IX_TempDeliverySlot_DeliveryPersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempPersonContact",
                table: "TempPersonContact",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempDeliverySlot",
                table: "TempDeliverySlot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TempDeliverySlot_TempDeliveryPersons_DeliveryPersonId",
                table: "TempDeliverySlot",
                column: "DeliveryPersonId",
                principalTable: "TempDeliveryPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TempPersonContact_TempDeliveryPersons_DeliveryPersonId",
                table: "TempPersonContact",
                column: "DeliveryPersonId",
                principalTable: "TempDeliveryPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
