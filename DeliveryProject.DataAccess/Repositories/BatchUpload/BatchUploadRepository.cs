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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<BatchUploadRepository> _logger;

        public BatchUploadRepository(IServiceScopeFactory serviceScopeFactory, ILogger<BatchUploadRepository> logger)
        {
            _serviceScopeFactory  = serviceScopeFactory;
            _logger = logger;
        }

        public async Task<List<Entities.BatchUpload>> GetPendingUploadsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
            
            return await dbContext.BatchUploads
                .Where(u => u.Status == UploadStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(Entities.BatchUpload batchUpload)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
            
            await dbContext.BatchUploads.AddAsync(batchUpload);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Entities.BatchUpload upload)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();

            dbContext.BatchUploads.Update(upload);
            await dbContext.SaveChangesAsync();
        }

        public async Task InsertIntoTempTableAsync<TDto, TEntity>(List<TDto> validRecords,
            Func<TDto, TEntity> entityMapper
        ) where TEntity : class
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();

            var entities = validRecords.Select(entityMapper).ToList();

            await dbContext.BulkInsertAsync(entities);
        }

        public async Task SaveErrorsAsync(List<UploadError> errorEntities)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();

            await dbContext.BulkInsertAsync(errorEntities);
        }

        public async Task<HashSet<string>> GetExistingPhoneNumbersAsync<TPerson>(List<string> phoneNumbers) where TPerson : PersonEntity
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
            
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
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DeliveryDbContext>();
                
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
