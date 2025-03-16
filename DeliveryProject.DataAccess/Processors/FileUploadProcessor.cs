using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Extensions;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DeliveryProject.Core.Constants.InfoMessages;
using DeliveryProject.Core.Constants.ErrorMessages;
using DeliveryProject.DataAccess.Interfaces.BatchUploads;

namespace DeliveryProject.DataAccess.Processors
{
    public class FileUploadProcessor : IFileUploadProcessor
    {
        private readonly IBatchUploadRepository _batchUploadRepository;
        private readonly string _uploadFolder = "uploads";
        private readonly ILogger<FileUploadProcessor> _logger;

        public FileUploadProcessor(IBatchUploadRepository batchUploadRepository, ILogger<FileUploadProcessor> logger)
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

            var batchUpload = CreateBatchUpload(file, filePath, uploadType);
            await _batchUploadRepository.AddAsync(batchUpload);
            return batchUpload;
        }

        private BatchUpload CreateBatchUpload(IFormFile file, string filePath, UploadType uploadType)
        {
            return new BatchUpload
            {
                FileName = file.FileName,
                FilePath = filePath,
                UploadType = uploadType
            };
        }
    }
}
