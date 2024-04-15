using System.Collections.Concurrent;
using BillOfCar.Models;

namespace BillOfCar.Manager;

public class LogManager
{
    static LogManager _Instance = null;
    public static LogManager GetInstance()
    {
        if (_Instance == null)
            _Instance = new LogManager();

        return _Instance;
    }

    public ConcurrentQueue<Log> WaitToAddLogs = null;
    public int LogLeftCount => WaitToAddLogs.Count;
    protected LogManager()
    {
        WaitToAddLogs = new ConcurrentQueue<Log>();
    }

    public void AddLog(Log log)
    {
        WaitToAddLogs.Enqueue(log);
    }

    public bool GetLog(out Log log)
    {
        return WaitToAddLogs.TryDequeue(out log);
    }
}