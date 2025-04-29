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
        private readonly IDbContextFactory<DeliveryDbContext> _dbContextFactory;
        private readonly ILogger<BatchUploadRepository> _logger;

        public BatchUploadRepository(IDbContextFactory<DeliveryDbContext> dbContextFactory, ILogger<BatchUploadRepository> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        public async Task<List<Entities.BatchUpload>> GetPendingUploadsAsync()
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            return await dbContext.BatchUploads
                .Where(u => u.Status == UploadStatus.Pending)
                .ToListAsync();
        }

        public async Task UpdateAsync(Entities.BatchUpload upload)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            dbContext.BatchUploads.Update(upload);
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertIntoTempTableAsync<TDto, TEntity>(List<TDto> validRecords,
            Func<TDto, TEntity> entityMapper
        ) where TEntity : class
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var entities = validRecords.Select(entityMapper).ToList();

            await dbContext.BulkInsertAsync(entities);
        }

        public async Task SaveErrorsAsync(List<UploadError> errorEntities)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            await dbContext.BulkInsertAsync(errorEntities);
        }

        public async Task<HashSet<string>> GetExistingPhoneNumbersAsync<TPerson>(List<string> phoneNumbers) where TPerson : PersonEntity
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var phoneNumberAttributeId = await dbContext.Attributes
                .Where(a => a.Key == AttributeKey.PhoneNumber)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            var existingPhoneNumbers = await dbContext.AttributeValues
                .Where(av => av.AttributeId == phoneNumberAttributeId && phoneNumbers.Contains(av.Value)
                                                                      && av.Person is TPerson)
                .Select(av => av.Value)
                .ToListAsync();

            return new HashSet<string>(existingPhoneNumbers); 
        }

        public async Task ExecuteMergeProcedureAsync(string tableName)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            try
            {
                await dbContext.Database.ExecuteSqlRawAsync($"CALL Merge{tableName}();");
                _logger.LogInformation(BatchUploadInfoMessages.MergeIsCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, BatchUploadErrorMessages.MergeError);
            }
        }
    }
}
