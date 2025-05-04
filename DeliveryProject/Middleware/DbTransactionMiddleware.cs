using DeliveryProject.DataAccess;
using Microsoft.EntityFrameworkCore.Storage;

namespace DeliveryProject.Middleware;

public class DbTransactionMiddleware
{
    private readonly RequestDelegate _next;

    public DbTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, DeliveryDbContext dbContext)
    {
        IDbContextTransaction transaction = null;
        
        try
        {
            transaction = await dbContext.Database.BeginTransactionAsync();

            await _next(context);

            if (dbContext.ChangeTracker.HasChanges())
            {
                await dbContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync();
            }
            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }
}