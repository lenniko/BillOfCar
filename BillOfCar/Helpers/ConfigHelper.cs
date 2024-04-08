using BillOfCar.Models;
using Newtonsoft.Json;

namespace BillOfCar.Helpers;

public class ConfigHelper
{
    private static Dictionary<string, string> _config = new Dictionary<string, string>();
    private static Dictionary<string, string> _clientConfig = new Dictionary<string, string>();
    private static CarContext _context;
    private static List<int> BlackList = new List<int>();
    private static List<int> WhiteList = new List<int>();
    private static bool Initialed;

    public static void Init(CarContext context)
    {
        if (Initialed)
        {
            return;
        }

        var configs = context.Configs.Where(k => k.Id > 0).ToList();
        foreach (var config in configs)
        {
            _config.Add(config.Key, config.Value);
            if(!config.ServerOnly)
                _clientConfig.Add(config.Key, config.Value);
        }
        Console.WriteLine(JsonConvert.SerializeObject(_config));
        Initialed = true;
    }

    public static string Get(string key)
    {
        _config.TryGetValue(key, out var value);
        return value;
    }

    public static void Refresh()
    {
        if (!_config.TryGetValue("BlackList", out var blackList))
        {
            blackList = new string("");
        }
        BlackList = JsonConvert.DeserializeObject<List<int>>(blackList);

        if (!_config.TryGetValue("WhiteList", out var whiteList))
        {
            whiteList = new string("");
        }
        WhiteList = JsonConvert.DeserializeObject<List<int>>(whiteList);
        
    }
    
}