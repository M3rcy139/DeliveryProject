using System;

namespace DeliveryProject.Core.Exceptions
{
    public class BussinessArgumentException : Exception
    {
        public BussinessArgumentException(string message)
            : base(message)
        {
        }

        public BussinessArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
