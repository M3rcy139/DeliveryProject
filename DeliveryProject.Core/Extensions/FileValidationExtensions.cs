using Microsoft.AspNetCore.Http;
using DeliveryProject.Core.Exceptions;


namespace DeliveryProject.Core.Extensions
{
    public static class FileValidationExtensions
    {
        public static void ValidateFile(this IFormFile file, string errorMessage)
        {
            if (file == null || file.Length == 0)
            {
                throw new BussinessArgumentException(errorMessage);
            }
        }
    }
}
