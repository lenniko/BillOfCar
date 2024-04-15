using System.Diagnostics;
using System.Text;

namespace BillOfCar.Helpers;

public static class DebugHelper
{
    public static eLogLevel PrintLevel = eLogLevel.Debug;
    public enum eLogLevel
    {
        StackTrace = 0,
        Exception,
        Error,
        Warning,
        Debug,
        Info,
        All,
    }
    public const string LogFMT = "{0}\t[{1}]\t[{2}]\t: ";//时间-等级-进程
    public const string FileInfoFMT = " => {0} - {1}";//文件名-行数

    public static void LogInfo(string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Info)
            return;
        Log($"Info", val, args);
    }
    public static void Log(string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Debug)
            return;
        Log($"Debug", val, args);
    }
    public static void LogError(string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Error)
            return;
        Log($"Error", val, args);
    }
    public static void LogPid(int Pid, string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Debug)
            return;
        Log($"Debug-Pid_{Pid}", val, args);
    }

    public static void Log(Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(LogFMT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), eLogLevel.Exception.ToString(), System.Diagnostics.Process.GetCurrentProcess().Id);
        Console.Write(sb);
        Console.WriteLine(ex);
    }
    public const int PrintStack = 5;
    public static void LogFileLine(string val = "", params object[] args)
    {
        StackTrace st = new System.Diagnostics.StackTrace(true);
        if (st.GetFrames().Length > 1)
        {
            _Log(eLogLevel.Info.ToString(), val, false, args);
            for (int i = 1; i < Math.Min(PrintStack, st.GetFrames().Length); i++)
            {
                StackFrame sf = st.GetFrame(i);
                _Log(eLogLevel.StackTrace.ToString(), string.Format(FileInfoFMT, sf.GetFileName(), sf.GetFileLineNumber()), false, args);
            }
        }
        else
        {
            _Log(eLogLevel.Info.ToString(), val, false, args);
        }

    }

    static void _Log(string level, string val, bool log_stackinfo, params object[] args)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(LogFMT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), level, System.Diagnostics.Process.GetCurrentProcess().Id);
        if (args.Length > 0)
            sb.AppendFormat(val, args);
        else
            sb.Append(val);

        if (log_stackinfo)
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            Console.WriteLine("st.FrameCount=" + st.FrameCount);
            StackFrame sf = st.GetFrame(1);
            if (sf != null)
                sb.AppendFormat(FileInfoFMT, sf.GetFileName(), sf.GetFileLineNumber());
        }
        Console.WriteLine(sb);
    }
    public static void LogPidWarning(int Pid, string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Warning)
            return;
        Log(eLogLevel.Warning.ToString()+ $"-Pid_{ Pid}", val, args);
    }
    public static void LogWarning(string val, params object[] args)
    {
        if (PrintLevel < eLogLevel.Warning)
            return;
        Log(eLogLevel.Warning.ToString(), val, args);
    }

    static void Log(string level, string val, params object[] args)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(LogFMT, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), level, Process.GetCurrentProcess().Id);
        if (args.Length > 0)
            sb.AppendFormat(val, args);
        else
            sb.Append(val);
        Console.WriteLine(sb);
    }

    public static StringBuilder GetExceptionInfo(Exception ex)
    {
        StackTrace st = new StackTrace(ex, true);
        StackFrame[] sfs = st.GetFrames();
        StringBuilder sb = new StringBuilder();
        if (ex.InnerException != null)
        {
            sb.Append("\n===============InnerException=================");
            sb.Append(GetExceptionInfo(ex.InnerException));
            sb.Append("\n==============================================");
        }

        sb.Append("Exception:\n");
        sb.Append(ex.Message);
        sb.Append("\nStackTrace:\n");
        for (int i = 0; i<sfs.Length; i++)
        {
            string file_name = sfs[i].GetFileName();
            if (!string.IsNullOrEmpty(file_name))
            {
                sb.Append(sfs[i].GetMethod());
                sb.Append(" in " + file_name);
                sb.Append(" Line: ");
                sb.Append(sfs[i].GetFileLineNumber());
                sb.Append("\n");
            }
        }
        return sb;
    }
}