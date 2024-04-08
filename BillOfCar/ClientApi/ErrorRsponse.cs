using BillOfCar.Consts;
using BillOfCar.Interfaces;
using BillOfCar.Services;

namespace BillOfCar.ClientApi;

public class ErrorRsponse : IPacketBody
{
    public int ErrorCode { get; set; }
    public string Message { get; set; }
    public PacketIds PacketId => PacketIds.Common_InternalError;
}