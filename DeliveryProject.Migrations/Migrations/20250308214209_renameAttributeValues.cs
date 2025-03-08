using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class renameAttributeValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonAttributeValues_Attributes_AttributeId",
                table: "PersonAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonAttributeValues_Persons_PersonId",
                table: "PersonAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonAttributeValues",
                table: "PersonAttributeValues");

            migrationBuilder.RenameTable(
                name: "PersonAttributeValues",
                newName: "AttributeValues");

            migrationBuilder.RenameIndex(
                name: "IX_PersonAttributeValues_AttributeId",
                table: "AttributeValues",
                newName: "IX_AttributeValues_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValues_Attributes_AttributeId",
                table: "AttributeValues",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeValues_Persons_PersonId",
                table: "AttributeValues",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValues_Attributes_AttributeId",
                table: "AttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_AttributeValues_Persons_PersonId",
                table: "AttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues");

            migrationBuilder.RenameTable(
                name: "AttributeValues",
                newName: "PersonAttributeValues");

            migrationBuilder.RenameIndex(
                name: "IX_AttributeValues_AttributeId",
                table: "PersonAttributeValues",
                newName: "IX_PersonAttributeValues_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonAttributeValues",
                table: "PersonAttributeValues",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAttributeValues_Attributes_AttributeId",
                table: "PersonAttributeValues",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonAttributeValues_Persons_PersonId",
                table: "PersonAttributeValues",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
