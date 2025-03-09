using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class IdForAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_PersonId",
                table: "AttributeValues",
                column: "PersonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_AttributeValues_PersonId",
                table: "AttributeValues");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttributeValues",
                table: "AttributeValues",
                column: "PersonId");
        }
    }
}
