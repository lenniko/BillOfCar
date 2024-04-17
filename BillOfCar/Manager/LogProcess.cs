using System.Collections.Concurrent;
using System.Timers;
using BillOfCar.Consts;
using BillOfCar.Helpers;
using BillOfCar.Models;
using Microsoft.EntityFrameworkCore.Internal;
using Timer = System.Timers.Timer;

namespace BillOfCar.Manager;

public class LogProcess : BaseProcess
{
    public LogProcess(eProcessType _type= eProcessType.Log) : base(_type)
    {
        ExpectedProcessCost = 1000;
    }
    public override void Init()
    {
        DebugHelper.Log($"LogProcess PID={PID} Init");
        Inited = true;
    }

    const int logCount = 1000;
    List<Log> list = new List<Log>();
    public override async Task Update()
    {
        Console.WriteLine("日志记录");
        try
        {
            for (int i = 0; i < logCount; i++)
            {
                if (LogManager.GetInstance().GetLog(out var log))
                {
                    list.Add(log);
                }
                else
                    break;
            }
            if (list.Count>0)
            {
                using (var db_context = ProcessManager.contentFactory.CreateDbContext())
                {
                    db_context.AddRange(list);
                    db_context.SaveChanges();
                }
                if(list.Count >=25 || LogManager.GetInstance().LogLeftCount >= 50)
                    DebugHelper.Log($"[LogProcess] 记录了{list.Count}条日志 当前剩余{LogManager.GetInstance().LogLeftCount}条日志");
                list.Clear();
            }
        }
        catch (Exception ex)
        {
            DebugHelper.Log(ex);
        }
    }

    public override void OnDestroy()
    {
            
    }
}