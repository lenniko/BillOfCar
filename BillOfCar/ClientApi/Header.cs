using ProtoBuf;

namespace BillOfCar.ClientApi;

[ProtoContract]
public class Header
{
    [ProtoMember((1))]
    public int PacketId { get; set; }
    [ProtoMember(3)]
    public int Token { get; set; }
}