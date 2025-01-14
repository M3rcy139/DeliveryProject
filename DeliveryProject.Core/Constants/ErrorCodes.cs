namespace DeliveryProject.Core.Constants
{
    public static class ErrorCodes
    {
        public static class Order
        {
            public const string NoOrdersFound = "NO_ORDERS_FOUND";
            public const string OrdersInTimeRangeNotFound = "ORDERS_IN_TIME_RANGE_NOT_FOUND";
        }

        public static class Region
        {
            public const string MustNotBeEmpty = "REGION_MUST_NOT_BE_EMPTY";
            public const string NotFound = "REGION_NOT_FOUND";
        }

        public static class Supplier
        {
            public const string NotFound = "SUPPLIER_NOT_FOUND";
        }

        public static class DeliveryPerson
        {
            public const string NoAvailable = "NO_AVAILABLE_DELIVERYPERSONS";
        }
    }
}

