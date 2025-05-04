using DeliveryProject.DataAccess.Interfaces;

namespace DeliveryProject.DataAccess.Repositories.Common;

public class FileUploadRepository : IFileUploadRepository
{
    private readonly DeliveryDbContext _dbContext;

    public FileUploadRepository(DeliveryDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Entities.BatchUpload batchUpload)
    {
        await _dbContext.BatchUploads.AddAsync(batchUpload);
    }
}