using BillOfCar.Models;

namespace BillOfCar.Services;

public interface IUserService
{
    /// <summary>
    /// 邮箱注册
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> RegUserByEmailAsync(string email, string password);
    /// <summary>
    /// 电话号码注册
    /// </summary>
    /// <param name="telephone"></param>
    /// <returns></returns>
    bool RegUserByTelephone(string telephone);
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    bool LogUser(User user);
    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="UserId"></param>
    void DeleteUser(int UserId);
    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>
    User GetUser(int UserId);
    /// <summary>
    /// 更新用户信息
    /// </summary>
    /// <param name="user"></param>
    void UpdateUser(User user);
    /// <summary>
    /// 校验用户
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    bool ValidUser(User user);
    
}