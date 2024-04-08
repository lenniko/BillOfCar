using System.Reflection;
using BillOfCar.Attributes;

namespace BillOfCar.Models;

public class ServiceAction
{
    public int PacketId;
    public int ServiceId;
    public int MethodId;
    public MethodInfo Method;
    public MethodAttribute MethodAttribute;
    public Type ParameterType;
}