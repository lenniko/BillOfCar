using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BillOfCar.Models;

[Table("BillRecord")]
public class BillRecord
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; }
    public float amount { get; set; }
    public int BillType { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset Created { get; set; }
    public string Notes { get; set; }
    public bool IsDeleted { get; set; }
}