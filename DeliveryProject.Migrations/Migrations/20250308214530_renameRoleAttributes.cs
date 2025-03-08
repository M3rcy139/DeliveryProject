using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class renameRoleAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleAttributeValues_Attributes_AttributeId",
                table: "RoleAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAttributeValues_Roles_RoleId",
                table: "RoleAttributeValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleAttributeValues",
                table: "RoleAttributeValues");

            migrationBuilder.RenameTable(
                name: "RoleAttributeValues",
                newName: "RoleAttributes");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAttributeValues_AttributeId",
                table: "RoleAttributes",
                newName: "IX_RoleAttributes_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleAttributes",
                table: "RoleAttributes",
                columns: new[] { "RoleId", "AttributeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAttributes_Attributes_AttributeId",
                table: "RoleAttributes",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAttributes_Roles_RoleId",
                table: "RoleAttributes",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleAttributes_Attributes_AttributeId",
                table: "RoleAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleAttributes_Roles_RoleId",
                table: "RoleAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleAttributes",
                table: "RoleAttributes");

            migrationBuilder.RenameTable(
                name: "RoleAttributes",
                newName: "RoleAttributeValues");

            migrationBuilder.RenameIndex(
                name: "IX_RoleAttributes_AttributeId",
                table: "RoleAttributeValues",
                newName: "IX_RoleAttributeValues_AttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleAttributeValues",
                table: "RoleAttributeValues",
                columns: new[] { "RoleId", "AttributeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAttributeValues_Attributes_AttributeId",
                table: "RoleAttributeValues",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleAttributeValues_Roles_RoleId",
                table: "RoleAttributeValues",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
