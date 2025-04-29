using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddMergeDeliveryPersonsProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE PROCEDURE MergeDeliveryPersons()
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    INSERT INTO ""Persons"" (""Id"", ""Status"", ""RegionId"", ""RoleId"")
                    SELECT tdp.""Id"", 'Active', tdp.""RegionId"", tdp.""RoleId""
                    FROM ""TempDeliveryPersons"" tdp
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""Persons"" p WHERE p.""Id"" = tdp.""Id""
                    );

                    INSERT INTO ""AttributeValues"" (""Id"", ""PersonId"", ""AttributeId"", ""Value"")
                    SELECT tav.""Id"", tav.""DeliveryPersonId"", tav.""AttributeId"", tav.""Value""
                    FROM ""TempAttributeValues"" tav
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""AttributeValues"" av 
                        WHERE av.""PersonId"" = tav.""DeliveryPersonId"" 
                        AND av.""AttributeId"" = tav.""AttributeId""
                    );

                    INSERT INTO ""DeliverySlots"" (""Id"", ""SlotTime"", ""DeliveryPersonId"")
                    SELECT tds.""Id"", tds.""SlotTime"", tds.""DeliveryPersonId""
                    FROM ""TempDeliverySlots"" tds
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""DeliverySlots"" ds 
                        WHERE ds.""SlotTime"" = tds.""SlotTime"" 
                        AND ds.""DeliveryPersonId"" = tds.""DeliveryPersonId""
                    );

                    DELETE FROM ""TempDeliveryPersons"";
                    DELETE FROM ""TempAttributeValues"";
                    DELETE FROM ""TempDeliverySlots"";
                END;
                $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS MergeDeliveryPersons();");
        }
    }
}
