using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.Business.Extensions;

public static class UnitOfWorkExtensions
{
    public static async Task ExecuteInTransaction(this IUnitOfWork unitOfWork, Func<Task> operation, ILogger logger = null)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            await operation();
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            logger?.LogError(ex, ErrorMessages.TransactionFailed);
            throw;
        }
    }

    public static async Task<TResult> ExecuteInTransaction<TResult>(this IUnitOfWork unitOfWork, Func<Task<TResult>> operation, ILogger logger = null)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var result = await operation();
            await unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            logger?.LogError(ex, ErrorMessages.TransactionFailed);
            throw;
        }
    }
}
