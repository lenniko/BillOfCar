using Microsoft.IO;

namespace BillOfCar.ClientApi;

public class MemoryStreamManager
{
    private static readonly RecyclableMemoryStreamManager Manager = new RecyclableMemoryStreamManager();

    public static MemoryStream GetStream()
    {
        return Manager.GetStream();
    }
}

public static class RecyclableMemoryStreamManageExtensions
{
    public static void CopyToMemorySteam(this byte[] bytes, MemoryStream stream, int offset, int length)
    {
        stream.Write(bytes, offset, length);
        stream.Seek(0, SeekOrigin.Begin);
    }

    public static byte[] GetBytes(this MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        var data = new byte[stream.Length];
        int b = 0;
        var index = 0;
        while((b = stream.ReadByte()) >= 0)
        {
            data[index] = (byte)b;
            index++;
        }
        return data;
    }
}