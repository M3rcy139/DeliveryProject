using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Constants.ErrorMessages;

namespace DeliveryProject.Bussiness.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IBatchUploadRepository _batchUploadRepository;
        private readonly string _uploadFolder = "uploads";
        private readonly ILogger<FileUploadService> _logger;

        public FileUploadService(IBatchUploadRepository batchUploadRepository, ILogger<FileUploadService> logger)
        {
            _batchUploadRepository = batchUploadRepository;
            _logger = logger;
        }

        public async Task<BatchUpload> UploadFileAsync(IFormFile file, UploadType uploadType)
        {
            file.ValidateFile(BatchUploadErrorMessages.FileEmpty);

            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation(BatchUploadInfoMessages.FileIsSaved, file.FileName, filePath);

            var batchUpload = new BatchUpload
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadType = uploadType
            };

            await _batchUploadRepository.AddAsync(batchUpload);
            return batchUpload;
        }
    }
}
