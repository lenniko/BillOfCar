using BillOfCar.Consts;
using BillOfCar.Interfaces;

namespace BillOfCar.ClientApi.UserAction;

public class SendCodeRequest : IRequestPacket, IPacketBody
{
    public int MsgIndex { get; set; }
    public string Telephone { get; set; }
    public PacketIds PacketId => PacketIds.User_SendCode;
}

public class SendCodeResponse : IPacketBody
{
    public int ErrorCode { get; set; }
    public string Code { get; set; }
    public PacketIds PacketId => PacketIds.User_SendCode;
}