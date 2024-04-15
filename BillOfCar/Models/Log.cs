using System.ComponentModel.DataAnnotations.Schema;

namespace BillOfCar.Models;

[Table("Log")]
public class Log
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string PacketId { get; set; }
    public string Logs { get; set; }
    public string Level { get; set; }
    public double Time { get; set; }
    public DateTimeOffset Date { get; set; }
}

public class Level
{
    public const string Error = "Error";
    public const string Debug = "Debug";
    public const string Info = "Info";
    public const string Trace = "Trace";
}