using DeliveryProject.DataAccess;

namespace DeliveryProject.Tests.Helpers
{
    public static class TransactionHelper
    {
        public static async Task ExecuteInTransactionAsync(DeliveryDbContext context, Func<Task> action)
        {
            await using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await action();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                await transaction.RollbackAsync();
                throw; 
            }
        }
    }
}
