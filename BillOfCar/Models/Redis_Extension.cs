namespace BillOfCar.Models;

public static class Redis_Extension
{
    public const string Code = "CODE";  // 验证码
    public const string CodeLimit = "CodeLimit"; // 发送的验证码次数
    public const string UserInfo = "USER"; // 用户信息
    public const string CarInfo = "CAR"; // 车辆信息

    public static string GenerateKey(string type, string key)
    {
        return type + "_" + key;
    }
}
