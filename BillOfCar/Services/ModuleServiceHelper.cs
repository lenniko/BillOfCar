using System.Diagnostics;
using System.Reflection;
using System.Text;
using BillOfCar.Attributes;
using BillOfCar.ClientApi;
using BillOfCar.Consts;
using BillOfCar.Helpers;
using BillOfCar.Interfaces;
using BillOfCar.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BillOfCar.Services;

public class ModuleServiceHelper : IModuleServiceHelper
{
    private readonly CarContext _context;
    private readonly LogContext _logContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IModuleService[] _allServices;
    private static ServiceActionMeta[][] _serviceActions;
    protected readonly ILogger _logger;
    public SystemManager _SystemManager { get; set; }
    protected readonly EventManager _eventManager;
    private object _lock = new object();
    private int _userId = -1;
    public ModuleServiceHelper(
        IServiceCollection serviceCollection,
        CarContext context,
        LogContext logContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ModuleServiceHelper> logger)
    {
        _context = context;
        _logContext = logContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;

        var services = new List<IModuleService>();
        var moduleServiceType = typeof(IModuleService);
        var serviceCo = serviceCollection.Where(sc => moduleServiceType.IsAssignableFrom(sc.ServiceType));
        foreach (var sc in serviceCollection.Where(sc => moduleServiceType.IsAssignableFrom(sc.ServiceType)))
        {
            if (sc.ServiceType.GetCustomAttributes(typeof(ServiceAttribute)) == null)
            {
                continue;
            }

            var service = (IModuleService)httpContextAccessor.HttpContext.RequestServices.GetService(sc.ServiceType);
            services.Add(service);
        }

        var maxServiceId = services.Select(s => s.ServiceId).Prepend(0).Max();
        _allServices = new IModuleService[maxServiceId + 1];
        lock (_lock)
        {
            if (_serviceActions == null)
            {
                _serviceActions = new ServiceActionMeta[maxServiceId + 1][];
            }
        }

        foreach (var baseService in services)
        {
            baseService.SetServiceHelper(this);
            ResolveModule(baseService);
            _allServices[baseService.ServiceId] = baseService;
        }
    }
    public async Task<User> GetCurrentUser()
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == GetCurrentUserId());
        return user;                                                                                                                                                                                                                                                                                                                                                                                                                      
    }

    public int GetCurrentUserId()
    {
        if (_userId > 0)
        {
            return _userId;
        }
        if(_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationStr))
        {
            var token = authorizationStr.ToString().Split(" ")[1];
            _userId =  JwtHelper.GetUserId(token);
        }
        return _userId;
    }

    public IModuleService GetService(int serviceId)
    {
        var service = _allServices[serviceId];
        if (service == null)
        {
            throw new Exception($"Invalid Service {serviceId}");
        }

        return service;
    }

    public ServiceAction GetService(int serviceId, int MethodId)
    {
        throw new NotImplementedException();
    }

    public ServiceActionMeta GetActionMeta(int serviceId, int methodId)
    {
        var meta = _serviceActions[serviceId][methodId];
        if (meta == null)
        {
            throw new Exception($"Invalid methodId {methodId} serviceId {serviceId}");
        }

        return meta;
    }

    public object InvokeRequest(IModuleService service, ServiceAction action, int UserId, object parameter)
    {
        throw new NotImplementedException();
    }

    public object InvokeRequest(int packetId, object param)
    {
        return InvokeMethodById(packetId, GetCurrentUserId(), param);
    }

    public Task<Object> InvokeMethodById(int packetId, int userId, object param)
    {
        var serviceId = packetId / GamePacket.Factor;
        var methodId = packetId % GamePacket.Factor;
        var service = GetService(serviceId);
        var meta = service.GetActionMeta(methodId);
        var parameterType = meta.ParameterType;
        object parameter = null;
        if (parameterType != null)
        {
            if (param is string str)
            {
                parameter = JsonConvert.DeserializeObject(str, parameterType);
            }
            else if (param is JObject jb)
            {
                parameter = JsonConvert.DeserializeObject(jb.ToString(), parameterType);
            }
            
        }

        return InvokeMethod(service, meta, userId, (IPacketBody)parameter);
    }

    private async Task<Object> InvokeMethod(IModuleService service, ServiceActionMeta meta, int userId,
        object parameter)
    {
        if (meta.MethodAttributes.NeedAuth && userId < 0)
        {
            return new StatusCodeResult(401);
        }

        var sw = new Stopwatch();
        sw.Start();
        try
        {
            var result = await InvokeMethodImpl(service, meta, userId, parameter);
            sw.Stop();
            var logModel = new LogModel();
            await _logContext.Logs.AddAsync(new Log()
            {
                UserId = userId,
                PacketId = meta.PacketId.ToString(),
                Time = sw.Elapsed.TotalMilliseconds,
                Date = DatetimeHelper.Now,
                Level = Level.Info,
                Logs = logModel.AddProp("Request", parameter)
                    .AddProp("Response", result)
                    .Build()
            });
            await _logContext.SaveChangesAsync();
            return result;
        }
        catch (Exception e)
        {
            sw.Stop();
            throw e;
        }
    }

    private async Task<object> InvokeMethodImpl(IModuleService service, ServiceActionMeta meta, int userId, object parameter)
    {
        object result = null;
        try
        {
            var requestMsgIndex = -1;
            if (parameter is IRequestPacket packet)
            {
                requestMsgIndex = packet.MsgIndex;
            }
            

            object[] parameters = null;
            if (parameter != null)
            {
                parameters = new[] { parameter };
            }

            result = meta.Method.Invoke(service, parameters);
            if (result is Task<IPacketBody> task)
            {
                result = await task;
            }
            return result;
        }
        catch (Exception e)
        {
            result = new GamePacket()
            {
                Header = new Header()
                {
                    PacketId = (int) PacketIds.Common_InternalError
                },
                Body = new ErrorRsponse()
                {
                    ErrorCode = 1,
                    Message = "内部错误"
                }
            };
            return result;
        }
    }
    private void ResolveModule(IModuleService service)
    {
        var metas = _serviceActions[service.ServiceId];
        if (metas != null)
        {
            return;
        }

        lock (_lock)
        {
            metas = _serviceActions[service.ServiceId];
            if (metas != null)
            {
                return;
            }

            var cachedMethods = new Dictionary<int, ServiceActionMeta>();
            var methods = service.GetType().GetMethods();
            foreach (var method in methods)
            {
                if (method.IsPublic && !method.IsStatic)
                {
                    var methodAttribute = method.GetCustomAttribute<MethodAttribute>();
                    if (methodAttribute != null)
                    {
                        var methodId = methodAttribute.MethodId;
                        if (cachedMethods.ContainsKey(methodId))
                        {
                            throw new Exception($"{service.GetType().Name} Have Conflict Method {method.Name} : {methodId}");
                        }

                        var meta = new ServiceActionMeta()
                        {
                            PacketId = GamePacket.Factor * service.ServiceId + methodId,
                            ServiceId = service.ServiceId,
                            MethodId = methodId,
                            Method = method,
                            MethodAttributes = methodAttribute
                        };
                        var parameters = method.GetParameters();
                        if (parameters.Length > 0)
                        {
                            var parameter = parameters[0];
                            meta.ParameterType = parameter.ParameterType;
                        }
                        cachedMethods.Add(methodId, meta); 
                    }
                }
            }

            var maxMethodId = cachedMethods.Keys.Select(k => k).Prepend(0).Max();
            var collection = new ServiceActionMeta[maxMethodId + 1];
            foreach (var kv in cachedMethods)
            {
                collection[kv.Key] = kv.Value;
            }

            _serviceActions[service.ServiceId] = collection;
        }
    }
}