namespace DeliveryProject.Bussiness.Helpers;

public static class BodyTruncator
{
    private const int MaxBodyLength = 2048;

    public static string Truncate(string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.Length <= MaxBodyLength
            ? value
            : value.Substring(0, MaxBodyLength) + "...(truncated)";
    }
}
