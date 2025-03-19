namespace DeliveryProject.Core.Exceptions
{
    public class BussinessArgumentException : Exception
    {
        public string ErrorCode { get; }

        public BussinessArgumentException(string message)
            : base(message)
        {
        }

        public BussinessArgumentException(string message, string errorCode)
        : base(message)
        {
            ErrorCode = errorCode;
        }

        public BussinessArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public BussinessArgumentException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, ErrorCode: {ErrorCode}";
        }
    }
}
