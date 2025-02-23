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
using Microsoft.Extensions.Configuration;

namespace DeliveryProject.Bussiness.Services
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
                var records = await ReadCsvFileAsync(upload.FilePath);

                if (!records.Any())
                {
                    upload.Status = UploadStatus.Failed;
                    return;
                }

                var existingPhoneNumbers = await _batchUploadRepository
                    .GetExistingPhoneNumbersAsync(records.Select(r => r.PhoneNumber).ToList());

                var (validRecords, errorRecords) = ValidateRecords(records, existingPhoneNumbers);

                foreach (var batch in validRecords.Chunk(_batchSize))
                {
                    await SaveValidRecordsAsync(batch.ToList());
                }

                foreach (var batch in errorRecords.Chunk(_batchSize))
                {
                    await SaveErrorRecordsAsync(batch.ToList(), upload.Id);
                }

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
