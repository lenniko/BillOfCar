using BillOfCar.Consts;
using BillOfCar.Interfaces;

namespace BillOfCar.ClientApi;

public class UserRegisterByEmailRequest : IRequestPacket, IPacketBody
{
    public int MsgIndex { get; set; }
    public PacketIds PacketId { get; }
}

public class UserRegisterByEmailResponse : IPacketBody
{
    public int ErrorCode { get; set; }
    public PacketIds PacketId { get; }
}