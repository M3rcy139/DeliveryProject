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
using DeliveryProject.DataAccess.Mappings;
using DeliveryProject.Core.Enums;

namespace DeliveryProject.DataAccess.Processors
{
    public class BatchUploadProcessor : IBatchUploadProcessor
    {
        private readonly IBatchUploadRepository _batchUploadRepository;
        private readonly IAttributeRepository _attributeRepository;
        private readonly ILogger<BatchUploadProcessor> _logger;
        private readonly int _batchSize;
        private Dictionary<AttributeKey, int> _attributeIds;

        public BatchUploadProcessor(IBatchUploadRepository batchUploadRepository, ILogger<BatchUploadProcessor> logger,
            IConfiguration configuration, IAttributeRepository attributeRepository)
        {
            _batchUploadRepository = batchUploadRepository;
            _logger = logger;
            _batchSize = configuration.GetValue<int>("BatchProcessing:BatchSize");
            _attributeRepository = attributeRepository;
        }

        public async Task ProcessUploadAsync(BatchUpload upload)
        {
            upload.Status = UploadStatus.Processing;
            await _batchUploadRepository.UpdateAsync(upload);

            try
            {
                await GetAttributeIds();
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

        private async Task GetAttributeIds()
        {
            _attributeIds = await _attributeRepository.GetAttributeIdsByKeys(
                new[] { AttributeKey.Name, AttributeKey.PhoneNumber, AttributeKey.Email, AttributeKey.Rating });
        }

        private async Task ProcessBatchUploadAsync(BatchUpload upload)
        {
            var existingPhoneNumbers = new HashSet<string>();

            await foreach (var batch in ReadCsvFileInBatchesAsync(upload.FilePath, _batchSize))
            {
                var allPhoneNumbers = batch
                    .SelectMany(r => r.Attributes
                        .Where(a => a.AttributeId == (int)AttributeKey.PhoneNumber)
                        .Select(a => a.Value))
                    .ToList();

                var newPhoneNumbers = await _batchUploadRepository.GetExistingPhoneNumbersAsync(allPhoneNumbers, RoleType.DeliveryPerson);
                existingPhoneNumbers.UnionWith(newPhoneNumbers);

                var batchValidator = new BatchValidator<DeliveryPersonDto>(
                    new DeliveryPersonDtoValidator(existingPhoneNumbers),
                    r => r.ToCsvString()
                );

                var validationResult = await batchValidator.ValidateRecordsAsync(batch);

                await SaveErrorRecordsAsync(validationResult.ErrorRecords, upload.Id);
                await SaveValidRecordsAsync(validationResult.ValidRecords);
            }

            await _batchUploadRepository.ExecuteMergeProcedureAsync("DeliveryPersons");
        }

        private async IAsyncEnumerable<List<DeliveryPersonDto>> ReadCsvFileInBatchesAsync(string filePath, int batchSize)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                MissingFieldFound = null
            });

            csv.Context.RegisterClassMap<DeliveryPersonDtoMap>();

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
            if (!validRecords.Any()) return;

            var tempDeliveryPersons = validRecords.Select(dto => new TempDeliveryPerson
            {
                Id = Guid.NewGuid(),
                RegionId = dto.RegionId,
                RoleId = (int)RoleType.DeliveryPerson
            }).ToList();

            var tempAttributeValues = validRecords
                .SelectMany(dto => dto.Attributes
                    .Where(a => _attributeIds.ContainsKey((AttributeKey)a.AttributeId))
                    .Select(a => new TempAttributeValue
                    {
                        Id = Guid.NewGuid(),
                        AttributeId = a.AttributeId,
                        Value = a.Value,
                        DeliveryPersonId = tempDeliveryPersons
                            .FirstOrDefault(dp => dp.RegionId == dto.RegionId).Id 
                    }))
                .ToList();

            var tempDeliverySlots = validRecords
                .SelectMany(dto => dto.DeliverySlots
                    .Select(slot => new TempDeliverySlot
                    {
                        Id = Guid.NewGuid(),
                        SlotTime = DateTime.SpecifyKind(slot.SlotTime, DateTimeKind.Utc),
                        DeliveryPersonId = tempDeliveryPersons
                            .FirstOrDefault(dp => dp.RegionId == dto.RegionId).Id 
                    }))
                .ToList();

            await _batchUploadRepository.InsertIntoTempTableAsync(tempDeliveryPersons, x => x);
            await _batchUploadRepository.InsertIntoTempTableAsync(tempAttributeValues, x => x);
            await _batchUploadRepository.InsertIntoTempTableAsync(tempDeliverySlots, x => x);
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
