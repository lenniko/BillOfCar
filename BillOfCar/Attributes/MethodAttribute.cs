namespace BillOfCar.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class MethodAttribute : Attribute
{
    public MethodAttribute(int methodId, bool needAuth = true)
    {
        MethodId = methodId;
        NeedAuth = needAuth;
    }
    
    public int MethodId { get; set; }
    public bool NeedAuth { get; set; }
}