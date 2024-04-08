using System.Reflection;

namespace BillOfCar.Services;

public static class ServiceCollectionExtensions
{
    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        var modules = new List<string>();
        var assembly  = typeof(BaseService).Assembly;
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(IModuleService)) && !type.IsAbstract)
            {
                services.AddScoped(type);
            }
        }
    }
}