using BillOfCar.Attributes;
using BillOfCar.ClientApi;
using BillOfCar.Consts;
using BillOfCar.Interfaces;
using Newtonsoft.Json;

namespace BillOfCar.Services;

[Service(1)]
public class CarService : BaseService
{
    public CarService()
    {
        
    }
    
    [Method((int)PacketIds.Car_AddCar % GamePacket.Factor)]
    public async Task<IPacketBody> AddCar(AddCarRequest request)
    {
        Console.WriteLine(JsonConvert.SerializeObject(request.Car));
        return new AddCarResponse()
        {
            ErrorCode = 0,
        };
    }

    [Method((int)PacketIds.Car_DeleteCar % GamePacket.Factor)]
    public async Task<IPacketBody> DeleteCar()
    {
        return null;
    }

    [Method((int)PacketIds.Car_UpdateCar % GamePacket.Factor)]
    public async Task<IPacketBody> UpdateCar()
    {
        return null;
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
        _serviceHelper = moduleServiceHelper;
    }

    public ServiceActionMeta GetActionMeta(int methodId)
    {
        throw new NotImplementedException();
    }
}