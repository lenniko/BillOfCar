using System.Drawing;
using System.Drawing.Imaging;

namespace BillOfCar.Helpers;

public static class ImageHelper
{
    public static readonly List<Color> Colors = new List<Color>()
    {
        Color.White,
        Color.Black,
        Color.Red,
        Color.Yellow,
        Color.Aquamarine,
        Color.Coral
    };
    public static readonly List<Color> ColorsOp = new List<Color>()
    {
        Color.Black,
        Color.White,
        Color.Yellow,
        Color.Red,
        Color.Coral,
        Color.Aquamarine,
    };

    public static readonly List<string> Icon = new List<string>()
    {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N"
    };
    public static Bitmap Generate(int seed)
    {
        Bitmap bitmap = new Bitmap(500, 500);

        Graphics graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(Colors[seed % Colors.Count]);

        Font font = new Font("Arial", 100);
        SolidBrush brush = new SolidBrush(ColorsOp[seed % Colors.Count]);
        PointF pointF = new PointF(200, 200);
        graphics.DrawString(Icon[seed % Icon.Count], font, brush, pointF);
        return bitmap;
        
    }

    public static string SavaLocal(Bitmap bitmap)
    {
        string imagePath = Path.Combine(AppContext.BaseDirectory, "/Avatars");
        bitmap.Save(imagePath, ImageFormat.Jpeg);
        Console.WriteLine($"图片已保存");
        return imagePath;
    }

    public static string GenerateAvatar()
    {
        // var bitMap = Generate(DatetimeHelper.TimeStamp);
        // return SavaLocal(bitMap);
        return AppContext.BaseDirectory + "/Avatar";
    }
}