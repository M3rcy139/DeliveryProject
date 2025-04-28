namespace DeliveryProject.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class TransactionAttribute : Attribute
{
}