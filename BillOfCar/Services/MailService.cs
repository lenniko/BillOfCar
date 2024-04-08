using BillOfCar.Attributes;
using BillOfCar.ClientApi;

namespace BillOfCar.Services;

[Service(5)]
public class MailService : BaseService
{
    public MailService()
    {
        
    }

    public int ServiceId { get; }
    public int GetCurrentUserId()
    {
        throw new NotImplementedException();
    }

    public Task<object> InvokeMethodById(int packetId, int userId, byte[] bytes)
    {
        throw new NotImplementedException();
    }

    public void SetServiceHelper(IModuleServiceHelper moduleServiceHelper)
    {
        throw new NotImplementedException();
    }

    public ServiceActionMeta GetActionMeta(int methodId)
    {
        throw new NotImplementedException();
    }
}