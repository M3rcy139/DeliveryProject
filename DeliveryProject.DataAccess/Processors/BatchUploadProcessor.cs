using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Common;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Enums;
using DeliveryProject.DataAccess.Validation;
using DeliveryProject.Core.Constants.ErrorMessages;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using DeliveryProject.Core.Constants.InfoMessages;
using Microsoft.Extensions.Configuration;
using DeliveryProject.Core.Validators;

namespace DeliveryProject.DataAccess.Processors
{
    public class BatchUploadProcessor : IBatchUploadProcessor
    {
        private readonly IBatchUploadRepository _batchUploadRepository;
        private readonly ILogger<BatchUploadProcessor> _logger;
        private readonly int _batchSize;

        public BatchUploadProcessor(IBatchUploadRepository batchUploadRepository, ILogger<BatchUploadProcessor> logger,
            IConfiguration configuration)
        {
            _batchUploadRepository = batchUploadRepository;
            _logger = logger;
            _batchSize = configuration.GetValue<int>("BatchProcessing:BatchSize");
        }

        public async Task ProcessUploadAsync(BatchUpload upload)
        {
            upload.Status = UploadStatus.Processing;
            await _batchUploadRepository.UpdateAsync(upload);

            try
            {
                await ProcessBatchUploadAsync(upload);
                upload.Status = UploadStatus.Completed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, BatchUploadErrorMessages.UploadError, upload.Id);
                upload.Status = UploadStatus.Failed;
            }

            await _batchUploadRepository.UpdateAsync(upload);
            _logger.LogInformation(BatchUploadInfoMessages.ProcessIsCompleted);
        }

        private async Task ProcessBatchUploadAsync(BatchUpload upload)
        {
            var existingPhoneNumbers = new HashSet<string>();

            await foreach (var batch in ReadCsvFileInBatchesAsync(upload.FilePath, _batchSize))
            {
                var newPhoneNumbers = await _batchUploadRepository
                    .GetExistingPhoneNumbersAsync(batch.Select(r => r.PhoneNumber).ToList());

                existingPhoneNumbers.UnionWith(newPhoneNumbers);

                var batchValidator = new BatchValidator<DeliveryPersonDto>(
                    new DeliveryPersonDtoValidator(existingPhoneNumbers),
                    r => r.ToCsvString()
                );

                var validationResult = await batchValidator.ValidateRecordsAsync(batch);

                await SaveErrorRecordsAsync(validationResult.ErrorRecords, upload.Id);
                await SaveValidRecordsAsync(validationResult.ValidRecords);
            }

            await _batchUploadRepository.ExecuteMergeProcedureAsync(nameof(DeliveryPerson) + 's');
        }

        private async IAsyncEnumerable<List<DeliveryPersonDto>> ReadCsvFileInBatchesAsync(string filePath, int batchSize)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                MissingFieldFound = null
            });

            var batch = new List<DeliveryPersonDto>();

            await foreach (var record in csv.GetRecordsAsync<DeliveryPersonDto>())
            {
                batch.Add(record);

                if (batch.Count >= batchSize)
                {
                    yield return batch;
                    batch = new List<DeliveryPersonDto>();
                }
            }

            if (batch.Any())
            {
                yield return batch;
            }
        }

        private async Task SaveValidRecordsAsync(List<DeliveryPersonDto> validRecords)
        {
            await _batchUploadRepository.InsertIntoTempTableAsync(
                validRecords,
                dto => new TempDeliveryPerson
                {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Rating = dto.Rating
                }
            );
        }

        private async Task SaveErrorRecordsAsync(List<ValidationRecordsError> errorRecords, Guid batchUploadId)
        {
            await _batchUploadRepository.SaveErrorsAsync(
                errorRecords.Select(err => new UploadError
                {
                    BatchUploadId = batchUploadId,
                    RowData = err.RowData,
                    ErrorMessage = err.ErrorMessage
                }).ToList()
            );
        }
    }
}
