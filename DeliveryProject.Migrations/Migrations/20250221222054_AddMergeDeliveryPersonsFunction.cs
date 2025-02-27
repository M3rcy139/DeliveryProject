using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryProject.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddMergeDeliveryPersonsFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION MergeDeliveryPersons()
                RETURNS void AS $$ 
                BEGIN
                    PERFORM setval(
                        pg_get_serial_sequence('""DeliveryPersons""', 'Id'),
                        (SELECT COALESCE(MAX(""Id""), 1) FROM ""DeliveryPersons""), 
                        true
                    );

                    INSERT INTO ""DeliveryPersons"" (""Name"", ""PhoneNumber"", ""Rating"", ""DeliverySlots"")
                    SELECT DISTINCT ON (tdp.""PhoneNumber"") 
                           tdp.""Name"", 
                           tdp.""PhoneNumber"", 
                           tdp.""Rating"", 
                           tdp.""DeliverySlots""
                    FROM ""TempDeliveryPersons"" tdp
                    WHERE NOT EXISTS (
                        SELECT 1 FROM ""DeliveryPersons"" dp WHERE dp.""PhoneNumber"" = tdp.""PhoneNumber""
                    )
                    ORDER BY tdp.""PhoneNumber"";

                    DELETE FROM ""TempDeliveryPersons"";
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS MergeDeliveryPersons();");
        }
    }
}
