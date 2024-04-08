using BillOfCar.Consts;

namespace BillOfCar.Interfaces;

public interface IPacketBody
{
    PacketIds PacketId { get; }
}

public interface IRequestPacket
{
    int MsgIndex { get; set; }
}