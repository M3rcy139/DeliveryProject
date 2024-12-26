using System.Text;

namespace DeliveryProject.API.Extensions
{
    public static class ExceptionExtensions
    {
        public static string FullMessage(this Exception exception, bool needStackTrace = true)
        {
            var message = new StringBuilder();

            if (needStackTrace)
                message.AppendLine(exception.StackTrace);

            message.AppendLine(exception.Message);

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                message.AppendLine(exception.Message);
            }

            return message.ToString();
        }
    }
}
