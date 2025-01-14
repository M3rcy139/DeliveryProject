namespace DeliveryProject.Core.Constants
{
    public static class ErrorMessages
    {
        public static class Order
        {
            public const string NotFound = "No orders found.";
            public const string InTimeRangeNotFound = "No orders were found for the {0} area in the time range from {1} to {2}.";
            public const string NoInRegion = "No orders were found in this area ({0}).";
        }

        public static class Region
        {
            public const string MustNotBeEmpty = "The regionName field must not be empty.";
            public const string NotFound = "The region with the name {0} was not found.";
        }

        public static class Supplier
        {
            public const string NotFound = "The specified supplier was not found.";
        }

        public static class DeliveryPerson
        {
            public const string NoAvailable = "There are no available delivery persons at the specified time.";
        }

        public static class Validation
        {
            public const string ValidationFailed = "Validation error: {0}";
            public const string EmptyOrderObject = "An empty Order object.";

            public const string InvalidRegionId = "The RegionId must be greater than zero.";
            public const string InvalidWeight = "The weight of the order must be positive.";
            public const string PastDeliveryTime = "The delivery time cannot be in the past.";
        }

        public static class BussinessLogic
        {
            public const string BusinessLogicalException = "Business logical exception: {0}";
            public const string ArgumentativeException = "An argumentative exception: {0}";
        }

        public static class General
        {
            public const string UnexpectedError = "Unexpected error: {0}";
        }
    }
}
