using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.DataAccess.Initializers
{
    public class DatabaseInitializer
    {
        private readonly IDbContextFactory<DeliveryDbContext> _dbContextFactory;

        public DatabaseInitializer(IDbContextFactory<DeliveryDbContext> dbContextFactory, ILogger<DatabaseInitializer> logger)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task InitializeDatabaseAsync()
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            string createFunctionSql = @"
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
            $$ LANGUAGE plpgsql;";

            await dbContext.Database.ExecuteSqlRawAsync(createFunctionSql);
        }
    }
}
