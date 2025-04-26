using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.DataAccess.Enums;
using DeliveryProject.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Microsoft.Extensions.Logging;
using DeliveryProject.Core.Enums;
using DeliveryProject.DataAccess.Interfaces.BatchUploads;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryProject.DataAccess.Repositories.BatchUpload
{
    public class BatchUploadRepository : IBatchUploadRepository
    {
        private readonly DeliveryDbContext _dbContext;
        private readonly ILogger<BatchUploadRepository> _logger;

        public BatchUploadRepository(DeliveryDbContext dbContext, ILogger<BatchUploadRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Entities.BatchUpload>> GetPendingUploadsAsync()
        {
            return await _dbContext.BatchUploads
                .Where(u => u.Status == UploadStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(Entities.BatchUpload batchUpload)
        {
            await _dbContext.BatchUploads.AddAsync(batchUpload);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Entities.BatchUpload upload)
        {
            _dbContext.BatchUploads.Update(upload);
            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertIntoTempTableAsync<TDto, TEntity>(List<TDto> validRecords,
            Func<TDto, TEntity> entityMapper
        ) where TEntity : class
        {
            var entities = validRecords.Select(entityMapper).ToList();

            await _dbContext.BulkInsertAsync(entities);
        }

        public async Task SaveErrorsAsync(List<UploadError> errorEntities)
        {
            await _dbContext.BulkInsertAsync(errorEntities);
        }

        public async Task<HashSet<string>> GetExistingPhoneNumbersAsync<TPerson>(List<string> phoneNumbers) where TPerson : PersonEntity
        {
            var phoneNumberAttributeId = await _dbContext.Attributes
                .Where(a => a.Key == AttributeKey.PhoneNumber)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            var existingPhoneNumbers = await _dbContext.AttributeValues
                .Where(av => av.AttributeId == phoneNumberAttributeId && phoneNumbers.Contains(av.Value)
                    && av.Person is TPerson)
                .Select(av => av.Value)
                .ToListAsync();

            return new HashSet<string>(existingPhoneNumbers); 
        }

        public async Task ExecuteMergeProcedureAsync(string tableName)
        {
            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync($"CALL Merge{tableName}();");
                _logger.LogInformation(BatchUploadInfoMessages.MergeIsCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, BatchUploadErrorMessages.MergeError);
            }
        }
    }
}
