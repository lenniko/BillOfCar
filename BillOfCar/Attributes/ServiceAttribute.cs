namespace BillOfCar.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ServiceAttribute : Attribute
{
    public ServiceAttribute(int serviceId)
    {
        ServiceId = serviceId;
    }
    public int ServiceId { get; set; }
}