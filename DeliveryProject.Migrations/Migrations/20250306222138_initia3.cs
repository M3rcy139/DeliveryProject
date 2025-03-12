using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class initia3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "TempDeliveryPersons");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "TempDeliveryPersons",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
