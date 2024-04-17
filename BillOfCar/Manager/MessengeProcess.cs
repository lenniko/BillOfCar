using BillOfCar.Consts;
using BillOfCar.Models;
using Newtonsoft.Json;

namespace BillOfCar.Manager;

public class MessageProcess : BaseProcess
{
    public const string MessageQueue = "MessageQueue";
    public static List<Message> Messages = new List<Message>();
    public static void ProduceMsg(Message message)
    {
        RedisHelper.LPush<Message>(MessageQueue, message);
        Console.WriteLine($"新增消息 {message.UUID}");
    }
    
    public MessageProcess(eProcessType _type = eProcessType.Message) : base(_type)
    {
        ExpectedProcessCost = 2000;
    }

    public override void Init()
    {
        Console.WriteLine($"MessageProcess PID={PID} Init");
        Inited = true;
    }
    public override async Task Update()
    {
        Console.WriteLine("消息队列处理");
        try
        {
            using (var context = ProcessManager.contentFactory.CreateDbContext())
            {
                while (true)
                {
                    var message = RedisHelper.BRPop<Message>(5,MessageQueue);
                    if (message == null)
                    {
                        break;
                    }
                    Messages.Add(message);
                    Console.WriteLine(JsonConvert.SerializeObject(message));
                    if (Messages.Count > 1000)
                    {
                        context.AddRange(Messages);
                        context.SaveChanges();
                        Messages.Clear();
                        break;
                    }
                } 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}