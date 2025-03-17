using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Entities;

namespace DeliveryProject.DataAccess.Interfaces.BatchUploads
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
        Task<HashSet<string>> GetExistingPhoneNumbersAsync<TPerson>(List<string> phoneNumbers) where TPerson : PersonEntity;
        Task ExecuteMergeProcedureAsync(string tableName);
    }

}
