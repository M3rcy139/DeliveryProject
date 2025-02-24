using DeliveryProject.DataAccess.Entities;
using DeliveryProject.Core.Dto;
using DeliveryProject.Core.Common;
using DeliveryProject.Core.Models;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.DataAccess.Enums;
using DeliveryProject.Core.Constants.ErrorMessages;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using DeliveryProject.Core.Constants.InfoMessages;
using Microsoft.Extensions.Configuration;
using DeliveryProject.Bussiness.Validators;

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
            var records = await ReadCsvFileAsync(upload.FilePath);

            if (!records.Any())
            {
                upload.Status = UploadStatus.Failed;
                return;
            }

            var existingPhoneNumbers = await _batchUploadRepository
                .GetExistingPhoneNumbersAsync(records.Select(r => r.PhoneNumber).ToList());

            var validationResult = await ValidateRecordsAsync(records, existingPhoneNumbers);
            var validRecords = validationResult.ValidRecords;
            var errorRecords = validationResult.ErrorRecords;

            await SaveValidRecordsInBatchesAsync(validRecords);
            await SaveErrorRecordsInBatchesAsync(errorRecords, upload.Id);

            await _batchUploadRepository.ExecuteMergeProcedureAsync(nameof(DeliveryPerson) + 's');
        }

        private async Task SaveValidRecordsInBatchesAsync(List<DeliveryPersonDto> validRecords)
        {
            if (!validRecords.Any()) return;

            foreach (var batch in validRecords.Chunk(_batchSize))
            {
                await SaveValidRecordsAsync(batch.ToList());
            }
        }

        private async Task SaveErrorRecordsInBatchesAsync(List<ValidationRecordsError> errorRecords, Guid batchUploadId)
        {
            if (!errorRecords.Any()) return;

            foreach (var batch in errorRecords.Chunk(_batchSize))
            {
                await SaveErrorRecordsAsync(batch.ToList(), batchUploadId);
            }
        }

        private async Task<List<DeliveryPersonDto>> ReadCsvFileAsync(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = "\t",
                MissingFieldFound = null
            });

            return csv.GetRecords<DeliveryPersonDto>().ToList();
        }

        private async Task<ValidationRecordsResult<DeliveryPersonDto>> ValidateRecordsAsync(
            List<DeliveryPersonDto> records, HashSet<string> existingPhoneNumbers)
        {
            var validator = new DeliveryPersonDtoValidator(existingPhoneNumbers);
            var validationResult = new ValidationRecordsResult<DeliveryPersonDto>();

            var validationTasks = records.Select(async record =>
            {
                var result = await validator.ValidateAsync(record);
                if (!result.IsValid)
                {
                    validationResult.ErrorRecords.Add(new ValidationRecordsError
                    {
                        RowData = record.ToCsvString(),
                        ErrorMessage = string.Join(", ", result.Errors.Select(e => e.ErrorMessage))
                    });
                }
                else
                {
                    validationResult.ValidRecords.Add(record);
                }
            });

            await Task.WhenAll(validationTasks);

            return validationResult;
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
