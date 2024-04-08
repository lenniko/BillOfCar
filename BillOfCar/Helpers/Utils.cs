using System.CodeDom.Compiler;

namespace BillOfCar.Helpers;

public static class Utils
{
    private static List<int> Nums = new List<int>()
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9
    };
    
    public static string GeneratedCode()
    {
        Random random = new Random(DatetimeHelper.TimeStamp);
        string code = "";
        for (int i = 0; i < 4; i++)
        {
            code += Nums[random.Next() % 10].ToString();
        }
        return code;
    }
}