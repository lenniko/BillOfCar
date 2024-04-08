using BillOfCar.Consts;
using BillOfCar.Interfaces;
using BillOfCar.Models;

namespace BillOfCar.ClientApi;

public class AddCarRequest : IRequestPacket, IPacketBody
{
    public int MsgIndex { get; set; }
    public Car Car { get; set; }
    public PacketIds PacketId => PacketIds.Car_AddCar;
}

public class AddCarResponse : IPacketBody
{
    public int ErrorCode { get; set; }
    public Car Car { get; set; }
    public PacketIds PacketId => PacketIds.Car_AddCar;
}