using BillOfCar.Consts;
using BillOfCar.Interfaces;

namespace BillOfCar.ClientApi.UserAction;

public class UserBindPhoneRequest : IRequestPacket, IPacketBody
{
    public int MsgIndex { get; set; }
    public string Phone { get; set; }
    public string Code { get; set; }
    public PacketIds PacketId => PacketIds.User_BindPhone;
}

public class UserBindPhoneResponse : IPacketBody
{
    public int ErrorCode { get; set; }

    public PacketIds PacketId => PacketIds.User_BindPhone;
}