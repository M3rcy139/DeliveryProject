namespace DeliveryProject.Core.Constants.ErrorMessages
{
    public static class ErrorMessages
    {
        public const string OrderNotFound = "No orders found.";

        public const string InvoiceNotFound = "Invoice not found.";
        
        public const string RegionMustNotBeEmpty = "The regionName field must not be empty.";
        public const string RegionNotFound = "The region was not found.";

        public const string SupplierNotFound = "The supplier was not found.";

        public const string CustomerNotFound = "The customer was not found.";

        public const string NoAvailableDeliveryPersons = "There are no available delivery persons at the specified time.";

        public const string ProductNotFound = "The product was not found";

        public const string NotSupportedEntityType = "Unsupported entity type: {0}";

        public const string BusinessLogicalException = "Business logical exception: {0}";
        public const string ArgumentativeException = "An argumentative exception: {0}";
        public const string InvalidOperationException = "An invalid operation exception: {0}";
        

        public const string UnexpectedErrorWithMessage = "Unexpected error: {0}";

        public const string JsonEmpty = "A JSON string cannot be empty or null.";

        public const string UnknownPersonType = "Unknown person type: {0}";
        
        public const string TransactionFailed = "Transaction failed.";
        
        public const string DbContextIsNotInitialized = "The DbContext is not initialized. Call BeginTransactionAsync first.";
    }
}
