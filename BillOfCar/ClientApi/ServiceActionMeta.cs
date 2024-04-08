using System.Reflection;
using BillOfCar.Attributes;

namespace BillOfCar.ClientApi;

public class ServiceActionMeta
{
    public int PacketId;
    public int ServiceId;
    public int MethodId;
    public MethodInfo Method;
    public MethodAttribute MethodAttributes;
    public Type ParameterType;
}