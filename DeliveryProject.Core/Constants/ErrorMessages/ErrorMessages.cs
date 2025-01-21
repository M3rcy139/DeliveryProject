namespace DeliveryProject.Core.Constants.ErrorMessages
{
    public static class ErrorMessages
    {
        public const string OrderNotFound = "No orders found.";
        public const string OrderInTimeRangeNotFound = "No orders were found for the {0} area in the time range from {1} to {2}.";
        public const string NoOrderInRegion = "No orders were found in this area ({0}).";

        public const string RegionMustNotBeEmpty = "The regionName field must not be empty.";
        public const string RegionNotFound = "The region with the name {0} was not found.";

        public const string SupplierNotFound = "The specified supplier was not found.";

        public const string NoAvailableDeliveryPersons = "There are no available delivery persons at the specified time.";

        public const string BusinessLogicalException = "Business logical exception: {0}";
        public const string ArgumentativeException = "An argumentative exception: {0}";

        public const string UnexpectedError = "Unexpected error: {0}";

        public const string JsonEmpty = "A JSON string cannot be empty or null.";
    }
}
