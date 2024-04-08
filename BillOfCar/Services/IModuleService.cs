using BillOfCar.ClientApi;

namespace BillOfCar.Services;

public interface IModuleService
{
    int ServiceId { get; }
    int GetCurrentUserId();
    Task<object> InvokeMethodById(int packetId, int userId, byte[] bytes);
    void SetServiceHelper(IModuleServiceHelper moduleServiceHelper);
    ServiceActionMeta GetActionMeta(int methodId);
}