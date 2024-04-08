using BillOfCar.ClientApi;
using BillOfCar.Models;

namespace BillOfCar.Services;

public interface IModuleServiceHelper
{
    /// <summary>
    /// 获取当前用户
    /// </summary>
    /// <returns></returns>
    Task<User> GetCurrentUser();
    /// <summary>
    /// 获取当前用户ID
    /// </summary>
    /// <returns></returns>
    int GetCurrentUserId();

    /// <summary>
    /// 获取service层
    /// </summary>
    /// <param name="serviceId"></param>
    /// <returns></returns>
    IModuleService GetService(int serviceId);

    /// <summary>
    /// 获取对应方法
    /// </summary>
    /// <param name="serviceId"></param>
    /// <param name="MethodId"></param>
    /// <returns></returns>
    ServiceAction GetService(int serviceId, int MethodId);

    /// <summary>
    /// 获取对应的action属性
    /// </summary>
    /// <param name="serviceId"></param>
    /// <param name="methodId"></param>
    /// <returns></returns>
    ServiceActionMeta GetActionMeta(int serviceId, int methodId);
    
    /// <summary>
    /// 调用service层方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="UserId"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    object InvokeRequest(IModuleService service, ServiceAction action, int UserId,  Object parameter);

    /// <summary>
    /// 调用service层
    /// </summary>
    /// <param name="packetId"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    object InvokeRequest(int packetId, object bytes);

    /// <summary>
    /// 调用method方法
    /// </summary>
    /// <param name="packetId"></param>
    /// <param name="userId"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
    Task<Object> InvokeMethodById(int packetId, int userId, object param);
}