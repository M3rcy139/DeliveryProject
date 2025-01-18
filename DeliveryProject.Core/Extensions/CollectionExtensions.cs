namespace DeliveryProject.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count == 0 ? new List<T>() : source;
        }
    }
}
