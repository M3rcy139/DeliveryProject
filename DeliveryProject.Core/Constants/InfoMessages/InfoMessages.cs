namespace DeliveryProject.Core.Constants.InfoMessages
{
    public static class InfoMessages
    {
        public const string AddedOrder = "Added an order with an ID: {0}.";
        public const string AddedInvoice = "Added an invoice with for Order with ID: {0}.";
        public const string FoundInRegion = "{0} orders found for the {1} area in the range from {2} to {3}.";
        public const string AllOrdersReceived = "{0} orders received.";
        public const string UpdatedOrder = "Updated an order with an ID: {0}.";
        public const string UpdatedOrderStatus ="Updated an order status with an ID: {0}.";
        public const string DeletedOrder = "Deleted an order with an ID: {0}.";
        public const string DeletedInvoice = "Deleted an invoice with for Order with ID: {0}.";

        public const string ProcessingStarted = "Started processing region data for {0}.";

        public const string RequestProcessingComplete = "Completion of request processing. Path: {Path}, Method: {Method}, StatusCode: {StatusCode}";

        public const string ValidationSucceeded = "Validation succeeded!";

        public const string HttpRequest = "HTTP Request: {Id} {Method} {Path} Body: {Body}";
        public const string HttpResponse = "HTTP Response: {Id} {StatusCode} Duration: {Duration}ms Body: {Body}";
    }
}
