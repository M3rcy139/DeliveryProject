using DeliveryProject.Bussiness.Interfaces.Services;
using DeliveryProject.Core.Enums;
using DeliveryProject.Core.Models;
using DeliveryProject.Core.Dto;
using DeliveryProject.DataAccess.Interfaces;
using DeliveryProject.Core.Constants.ErrorMessages;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
using DeliveryProject.Core.Constants.InfoMessages;

namespace DeliveryProject.Bussiness.Services
{
    public class BatchUploadService : IBatchUploadService
    {
        private readonly IBatchUploadRepository _batchUploadRepository;
        private readonly ILogger<BatchUploadService> _logger;

        public BatchUploadService(IBatchUploadRepository batchUploadRepository, ILogger<BatchUploadService> logger)
        {
            _batchUploadRepository = batchUploadRepository;
            _logger = logger;
        }

        public async Task ProcessUploadAsync(BatchUpload upload)
        {
            upload.Status = UploadStatus.Processing;
            await _batchUploadRepository.UpdateAsync(upload);

            try
            {
                var records = await ReadCsvFileAsync(upload.FilePath);

                if (!records.Any())
                {
                    upload.Status = UploadStatus.Failed;
                    return;
                }

                var existingPhoneNumbers = await _batchUploadRepository
                    .GetExistingPhoneNumbersAsync(records.Select(r => r.PhoneNumber).ToList());

                var (validRecords, errorRecords) = ValidateRecords(records, existingPhoneNumbers);

                await SaveValidRecordsAsync(validRecords);
                await SaveErrorRecordsAsync(errorRecords, upload.Id);

                await _batchUploadRepository.ExecuteMergeProcedureAsync(nameof(DeliveryPerson) + 's');

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

        private (List<DeliveryPersonDto> validRecords, List<(string rowData, string errorMessage)> errorRecords)
        ValidateRecords(List<DeliveryPersonDto> records, HashSet<string> existingPhoneNumbers)
        {
            var validRecords = new List<DeliveryPersonDto>();
            var errorRecords = new List<(string rowData, string errorMessage)>();

            foreach (var record in records)
            {
                if (string.IsNullOrWhiteSpace(record.Name))
                {
                    errorRecords.Add((record.ToCsvString(), BatchUploadErrorMessages.IncorrectData));
                }
                else if (existingPhoneNumbers.Contains(record.PhoneNumber))
                {
                    errorRecords.Add((record.ToCsvString(), BatchUploadErrorMessages.AlreadyExists));
                }
                else
                {
                    validRecords.Add(record);
                }
            }

            return (validRecords, errorRecords);
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

        private async Task SaveErrorRecordsAsync(List<(string rowData, string errorMessage)> errorRecords, Guid batchUploadId)
        {
            await _batchUploadRepository.SaveErrorsAsync(
                errorRecords.Select(err => new UploadError
                {
                    BatchUploadId = batchUploadId,
                    RowData = err.rowData,
                    ErrorMessage = err.errorMessage
                }).ToList()
            );
        }
    }
}
