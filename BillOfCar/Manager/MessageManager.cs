using System.Collections.Concurrent;
using BillOfCar.Models;

namespace BillOfCar.Manager;

public class MessageManager
{
    public static ConcurrentQueue<Message> Messages;
    public const string MessageKey = "MessageQueue";
    public static void ProduceMsg(Message message)
    {
        RedisHelper.LPush<Message>(MessageKey, message);
    }

    public static bool ConsumeMsg(out Message message)
    {
        return Messages.TryDequeue(out message);
    }
}