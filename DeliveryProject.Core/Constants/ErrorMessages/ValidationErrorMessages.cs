namespace DeliveryProject.Core.Constants.ErrorMessages
{
    public static class ValidationErrorMessages
    {
        public const string GenericValidationFailed = "Validation failed.";
        public const string ValidationFailed = "Validation error: {0}";
        public const string EmptyOrderObject = "An empty Order object.";

        public const string InvalidRegionId = "The RegionId must be greater than zero.";
        public const string InvalidWeight = "The weight of the order must be positive.";
        public const string PastDeliveryTime = "The delivery time cannot be in the past.";

        public const string QuantityLength = "Quantity should be from 1 to 100.";
        
        public const string RequiredOrderId = "OrderId field is required.";
        public const string RequiredCustomerId = "CustomerId field is required.";
        public const string RequiredProducts = "Products field is required.";
        public const string ProductListNotEmpty = "Products list should not be empty.";
    }
}
