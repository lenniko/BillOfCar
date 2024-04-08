using System.Diagnostics;
using BillOfCar.ClientApi;
using BillOfCar.Helpers;
using BillOfCar.Interfaces;
using BillOfCar.Models;
using BillOfCar.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProtoBuf;

namespace BillOfCar.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class SystemController : Controller
{
    private readonly ILogger<SystemController> _logger;
    private readonly IModuleServiceHelper _moduleServiceHelper;
    private readonly CarContext _context;
    private readonly LogContext _logContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public SystemController(IHttpContextAccessor httpContextAccessor,
        IModuleServiceHelper moduleServiceHelper,
        CarContext context,
        LogContext logContext,
        ILogger<SystemController> logger)
    {
        _logger = logger;
        _logContext = logContext;
        _moduleServiceHelper = moduleServiceHelper;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    [HttpPost]
    public async Task<IActionResult> Packet()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        int userId = _moduleServiceHelper.GetCurrentUserId();
        var packet = new GamePacket();
        try
        {
            using (var ms = MemoryStreamManager.GetStream())
            {
                await _httpContextAccessor.HttpContext.Request.Body.CopyToAsync(ms);
                var data = ms.GetBytes();
                packet.Decode(data, 0, data.Length);
            }

            var header = packet.Header;
            var packetId = header.PacketId;
            try
            {
                var result = _moduleServiceHelper.InvokeRequest(packetId, packet.Body);
                if (result is Task<object> taskObj)
                {
                    result = await taskObj;
                }

                if (result is Task<IPacketBody> task)
                {
                    result = await task;
                }

                if (result is IPacketBody)
                {
                    var data = new GamePacket(header, result).Encode();
                    return new FileContentResult(data, "application/octer-stream");
                }

                if (result is GamePacket gamePacket)
                {
                    var data = gamePacket.Encode();
                    return new FileContentResult(data, "application/octer-stream");
                }

                if (result is IActionResult actionResult)
                {
                    return actionResult;
                }
            }
            catch (Exception e)
            {
                var logModel = new LogModel();
                await _logContext.Logs.AddAsync(new Log()
                {
                    PacketId = packetId.ToString(),
                    Time = -1,
                    UserId = userId,
                    Logs = logModel.AddProp("Request", packet).AddProp("Exception", e)
                        .Build(),
                    Date = DatetimeHelper.Now
                });
                await _logContext.SaveChangesAsync();
            }

            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(JsonConvert.SerializeObject(e));
        }
        return NoContent();
    }
}