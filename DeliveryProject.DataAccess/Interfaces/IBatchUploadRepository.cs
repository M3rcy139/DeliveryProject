using DeliveryProject.Core.Models;

namespace DeliveryProject.DataAccess.Interfaces
{
    public interface IBatchUploadRepository
    {
        Task<List<BatchUpload>> GetPendingUploadsAsync();
        Task AddAsync(BatchUpload batchUpload);
        Task UpdateAsync(BatchUpload batchUpload);
        Task InsertIntoTempTableAsync<TDto, TEntity>(List<TDto> validRecords,
            Func<TDto, TEntity> entityMapper
        ) where TEntity : class;
        Task SaveErrorsAsync(List<UploadError> errorEntities);
        Task<HashSet<string>> GetExistingPhoneNumbersAsync(List<string> phoneNumbers);
        Task ExecuteMergeProcedureAsync(string tableName);
    }

}
