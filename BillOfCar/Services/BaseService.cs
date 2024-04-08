using BillOfCar.Attributes;
using BillOfCar.ClientApi;
using BillOfCar.Models;

namespace BillOfCar.Services;

public abstract class BaseService : IModuleService
{
    private int _serviceId = -1;
    protected IModuleServiceHelper _serviceHelper;
    protected readonly CarContext _context;
    protected readonly ILogger _logger;

    public int ServiceId
    {
        get
        {
            if (_serviceId == -1)
            {
                var serviceAttribute = GetType().GetCustomAttributes(typeof(ServiceAttribute), true);
                if (serviceAttribute.Length > 0)
                {
                    _serviceId = ((ServiceAttribute)serviceAttribute[0]).ServiceId;
                }
            }
            return _serviceId;
        }
    }
    public int GetCurrentUserId()
    {
        return _serviceHelper.GetCurrentUserId();
    }

    public Task<object> InvokeMethodById(int packetId, int userId, byte[] bytes)
    {
        return null;
    }

    public void SetServiceHelper(IModuleServiceHelper moduleServiceHelper)
    {
        this._serviceHelper = moduleServiceHelper;
    }

    public ServiceActionMeta GetActionMeta(int methodId)
    {
        return _serviceHelper.GetActionMeta(this.ServiceId, methodId);
    }
}