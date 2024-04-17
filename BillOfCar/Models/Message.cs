using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace BillOfCar.Models;

[Table("Message")]
public class Message
{
    [Key]
    public string UUID { get; set; }
    [Column]
    public eMessage Type { get; set; }
    [Column]
    public int UserId { get; set; }
    [Column]
    public string Content { get; set; }
    [Column]
    public int Timestamp { get; set; }
}

public enum eMessage
{
    Notice = 1, // 通知
    Email = 2, // 邮件通知
}