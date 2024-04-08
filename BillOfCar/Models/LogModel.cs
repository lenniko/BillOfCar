using Newtonsoft.Json;

namespace BillOfCar.Models;

public class LogModel
{
    private Dictionary<string, object> Prop = new Dictionary<string, object>();
    public string Build()
    {
        return JsonConvert.SerializeObject(Prop);
    }

    public LogModel AddProp(string key, object value)
    {
        if (!Prop.TryGetValue(key, out var _out))
        {
            Prop.Add(key, value);
        }

        return this;
    }
}