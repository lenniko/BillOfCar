using System.Text;
using Newtonsoft.Json;

namespace BillOfCar.ClientApi;

public class GamePacket : PacketFormat
{
    public const int Factor = 1000;
    [JsonProperty]
    public Header Header { get; set; }
    [JsonProperty]
    public object Body { get; set; }

    public GamePacket(){ }
    public GamePacket(Header h, object b)
    {
        Header = h;
        Body = b;
    }
    
    public override int Decode(byte[] bytes, int offset, int available)
    {
        var str = Encoding.UTF8.GetString(bytes);
        var packet = JsonConvert.DeserializeObject<GamePacket>(str);
        if (packet != null && packet.Header != null && packet.Body != null)
        {
            this.Header = packet.Header;
            this.Body = packet.Body;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public override byte[] Encode()
    {
        var str = JsonConvert.SerializeObject(this);
        byte[] bytes = Encoding.UTF8.GetBytes(str);
        return bytes;
    }

    public override bool IsLoaded()
    {
        throw new NotImplementedException();
    }
}