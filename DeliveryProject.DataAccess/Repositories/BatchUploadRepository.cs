using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryProject.DataAccess.Repositories
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

        public async Task<List<BatchUpload>> GetPendingUploadsAsync()
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            return await dbContext.BatchUploads
                .Where(u => u.Status == UploadStatus.Pending)
                .ToListAsync();
        }

        public async Task AddAsync(BatchUpload batchUpload)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            await dbContext.BatchUploads.AddAsync(batchUpload);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BatchUpload upload)
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

            if (!validRecords.Any()) return;

            var entities = validRecords.Select(entityMapper).ToList();
            var dbSet = dbContext.Set<TEntity>();

            await dbSet.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveErrorsAsync(List<UploadError> errorEntities)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            
            if (!errorEntities.Any()) return;

            await dbContext.UploadErrors.AddRangeAsync(errorEntities);
            await dbContext.SaveChangesAsync();
        }

        public async Task<HashSet<string>> GetExistingPhoneNumbersAsync(List<string> phoneNumbers)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            var phoneNumbersList = await dbContext.DeliveryPersons
                .Where(dp => phoneNumbers.Contains(dp.PhoneNumber))
                .Select(dp => dp.PhoneNumber)
                .ToListAsync(); 

            return new HashSet<string>(phoneNumbersList); 
        }

        public async Task ExecuteMergeProcedureAsync(string tableName)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            try
            {
                await dbContext.Database.ExecuteSqlRawAsync($"DO $$ BEGIN PERFORM Merge{tableName}(); END $$;");
                _logger.LogInformation(BatchUploadInfoMessages.MergeIsCompleted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, BatchUploadErrorMessages.MergeError);
            }
        }
    }
}
