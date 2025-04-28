using DeliveryProject.Attributes;
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
        if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        
        var endpoint = context.GetEndpoint();
        var hasTransactionAttribute = endpoint?.Metadata.GetMetadata<TransactionAttribute>() != null;
        
        if (!hasTransactionAttribute)
        {
            await _next(context);
            return;
        }

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