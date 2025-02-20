using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DeliveryProject.Core.Constants.ErrorMessages
{
    public static class BatchUploadErrorMessages
    {
        public const string IncorrectData = "Incorrect data";
        public const string AlreadyExists = "The record already exists in the database";
        public const string UploadError = "Error processing the upload {0}";
        public const string FileEmpty = "The file is empty or missing";

        public const string MergeError = "Error executing merge procedure";
    }
}
