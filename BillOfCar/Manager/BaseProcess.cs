using System.Diagnostics;
using BillOfCar.Consts;
using BillOfCar.Helpers;
using BillOfCar.Interfaces;

namespace BillOfCar.Manager;

public class BaseProcess: IProcess
    {
        public int PID;
        public Task task;
        public eProcessType ProcessType;
        public long CurProcessCost = 0;//以33ms作为间隔 
        public long ExpectedProcessCost = 100;//以33ms作为间隔 
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        public BaseProcess(eProcessType _type)
        {

            ProcessType = _type;
            CancellationToken token = tokenSource.Token;

            task = new Task(async () =>
            {
                Stopwatch sw = new Stopwatch();
                
                while (true)
                {
                    if (token.IsCancellationRequested)
                    {
                        OnDestroy();
                        return;
                    }
                    if (!Inited)
                        Init();

                    sw.Restart();
                    await Update();
                    sw.Stop();
                    CurProcessCost = sw.ElapsedMilliseconds;
                    long wait = ExpectedProcessCost - CurProcessCost;
                    if (wait >= 0)
                        await Task.Delay((int)wait);

                    if (CurProcessCost >= ExpectedProcessCost)
                    {
                        DebugHelper.Log($"{ProcessType} 进程{PID} 每帧耗时 {CurProcessCost}ms，大于期望值 {ExpectedProcessCost}ms");
                    }
                }
            });
            PID = task.Id;
            Console.WriteLine($"创建日志线程{PID}");
            ProcessManager.GetInstance().Dict_Process.TryAdd(PID, this);
        }
        public bool IsInited => Inited;
        protected bool Inited=false;
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Init()
        {
            Inited = true;
            Console.WriteLine($"{PID} Init");
            // DebugHelper.Log($"pid={PID} Init!");
        }

        public virtual async Task Update()
        {
            DebugHelper.Log($"pid={PID} Update!");
        }
        public virtual void OnDestroy()
        {
            //自我清理相关占用内存
            DebugHelper.Log($"pid={PID} OnDestroy!");
        }

        public virtual void Destroy()
        {
            DebugHelper.Log($"pid={PID} Destroy!");
            tokenSource.Cancel();
        }
        public virtual object GetSelfInfo(int UserId)
        {
            return null;
        }
        public virtual object GetLastFrameDataByUserId(int UserId, float Range)
        {
            return null;
        }

        //从某帧开始，获取userid相关的 一共 frames 帧的数据
        public virtual object GetFrameDataByUserId(int StartFrame, int FramesCount, int UserId, float Range)
        {
            return null;
        }
        public virtual void UnregisterListeners() { }
        public virtual void RegisterListeners() { }
    }