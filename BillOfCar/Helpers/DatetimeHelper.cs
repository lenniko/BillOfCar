namespace BillOfCar.Helpers;

public static class DatetimeHelper
{
    public static readonly DateTimeOffset BaseTime = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
    
    /// <summary>
    /// Utc的当前时间
    /// </summary>
    public static DateTimeOffset Now => DateTimeOffset.UtcNow;

    /// <summary>
    /// 获取对应时间戳
    /// </summary>
    /// <param name="dateTimeOffset"></param>
    /// <returns></returns>
    public static int GetTimeStamp(DateTimeOffset dateTimeOffset)
    {
        return (int)dateTimeOffset.Subtract(BaseTime).TotalSeconds;
    }

    /// <summary>
    /// 转化时间戳
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTimeOffset ParseTimeStamp(int timeStamp)
    {
        return BaseTime.AddSeconds(timeStamp);
    }
    
    /// <summary>
    /// 当前时间戳 秒
    /// </summary>
    public static int TimeStamp => (int)(Now.Subtract(BaseTime)).TotalSeconds;

    /// <summary>
    /// 当前时间戳 毫秒
    /// </summary>
    public static long NowMilliseconds => (long)(Now.Subtract(BaseTime)).TotalMilliseconds;
}