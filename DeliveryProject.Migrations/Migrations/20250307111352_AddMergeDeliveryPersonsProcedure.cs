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
                    INSERT INTO ""Persons"" (""Id"", ""Name"", ""RoleId"")
                    SELECT tdp.""Id"", tdp.""Name"", 3
                    FROM ""TempDeliveryPersons"" tdp
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""Persons"" p WHERE p.""Id"" = tdp.""Id""
                    );

                    INSERT INTO ""DeliveryPersons"" (""Id"", ""Rating"")
                    SELECT tdp.""Id"", tdp.""Rating""
                    FROM ""TempDeliveryPersons"" tdp
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""DeliveryPersons"" dp WHERE dp.""Id"" = tdp.""Id""
                    );

                    INSERT INTO ""PersonContacts"" (""Id"", ""PhoneNumber"", ""Email"", ""RegionId"", ""PersonId"")
                    SELECT tpc.""Id"", tpc.""PhoneNumber"", tpc.""Email"", tpc.""RegionId"", tpc.""DeliveryPersonId""
                    FROM ""TempPersonContacts"" tpc
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM ""PersonContacts"" pc 
                        WHERE pc.""PhoneNumber"" = tpc.""PhoneNumber"" 
                        AND pc.""PersonId"" = tpc.""DeliveryPersonId""
                    );

                    INSERT INTO ""DeliverySlots"" (""Id"", ""SlotTime"", ""DeliveryPersonId"")
                    SELECT tds.""Id"", tds.""SlotTime"", tds.""DeliveryPersonId""
                    FROM ""TempDeliverySlots"" tds
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM ""DeliverySlots"" ds 
                        WHERE ds.""SlotTime"" = tds.""SlotTime"" 
                        AND ds.""DeliveryPersonId"" = tds.""DeliveryPersonId""
                    );

                    DELETE FROM ""TempDeliveryPersons"";
                    DELETE FROM ""TempPersonContacts"";
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
