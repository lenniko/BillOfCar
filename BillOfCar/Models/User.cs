using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BillOfCar.Consts;

namespace BillOfCar.Models;

[Table("User")]
public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public eRole Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    
}