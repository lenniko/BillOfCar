using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BillOfCar.Models;

[Table("Config")]
public class Config
{
    [Key]
    public int Id { get; set; }
    [Column]
    public string Key { get; set; }
    [Column]
    public string Value { get; set; }
    [Column]
    public bool ServerOnly { get; set; }
    [Column]
    public bool Valid { get; set; }
}