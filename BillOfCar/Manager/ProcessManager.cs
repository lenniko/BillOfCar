using System.Collections.Concurrent;
using BillOfCar.Consts;
using BillOfCar.Helpers;
using BillOfCar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BillOfCar.Manager;

public class ProcessManager
    {
        static ProcessManager _Instance = null;
        public static ProcessManager GetInstance()
        {
            if (_Instance == null)
                _Instance = new ProcessManager();

            return _Instance;
        }

        public static IDbContextFactory<LogContext> contentFactory;
        public static IHttpClientFactory HttpClientFactory { get; set; }
        public ConcurrentDictionary<int, BaseProcess> Dict_Process = new ConcurrentDictionary<int, BaseProcess>();
        ProcessManager()
        {
            Console.WriteLine("线程管理器启动");
            _ = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    
                    try
                    {
                        var baseProcess = Dict_Process.Values.FirstOrDefault(_t => _t.task.Status == TaskStatus.Created);
                        if (baseProcess != null)
                        {
                            baseProcess.task.Start();
                            // DebugHelper.Log($"[ProcessManager] PID={baseProcess.PID} ProcessType={baseProcess.ProcessType} 正式启动");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("错误" + ex);
                        // DebugHelper.Log(ex);
                    }
                    await Task.Delay(5000);
                }
            });
        }
    }