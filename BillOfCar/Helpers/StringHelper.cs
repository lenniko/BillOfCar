using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BillOfCar.Helpers;

public static class StringHelper
{
    /// <summary>
    /// 校验邮箱
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(email);
    }

    /// <summary>
    /// 校验密码
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public static bool IsValidPassword(string password)
    {
        if (password.Length > 20)
        {
            return false;
        }
        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
        Regex regex = new Regex(pattern);
        return regex.IsMatch(password);
    }
    
    /// <summary>
    /// 生成MD5
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static string MD5(string origin)
    {
        var md5 = System.Security.Cryptography.MD5.Create();
        var bytes = Encoding.ASCII.GetBytes(origin);
        var computeHash = md5.ComputeHash(bytes);
        var result = BitConverter.ToString(computeHash);
        result = result.Replace("-", "").ToLower();
        return result;
    }
}