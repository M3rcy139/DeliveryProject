using DeliveryProject.Core.Common;
using FluentValidation;

namespace DeliveryProject.DataAccess.Validation
{
    public class BatchValidator<T>
    {
        private readonly IValidator<T> _validator;
        private readonly Func<T, string> _toRowData;

        public BatchValidator(IValidator<T> validator, Func<T, string> toRowData)
        {
            _validator = validator;
            _toRowData = toRowData;
        }

        public async Task<ValidationRecordsResult<T>> ValidateRecordsAsync(List<T> records)
        {
            var validationResult = new ValidationRecordsResult<T>();

            var validationTasks = records.Select(async record =>
            {
                var result = await _validator.ValidateAsync(record);
                if (!result.IsValid)
                {
                    validationResult.ErrorRecords.Add(new ValidationRecordsError
                    {
                        RowData = _toRowData(record),
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
    }
}
