using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BillOfCar.Consts;

namespace BillOfCar.Models;

[Table("Car")]
public class Car
{
    [Key]
    public int Id { get; set; }
    public string Brand { get; set; }
    public string? Model { get; set; }
    public string Number { get; set; }
    public eCarType Type { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool IsDeleted { get; set; }
}