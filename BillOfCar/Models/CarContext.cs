using Microsoft.EntityFrameworkCore;

namespace BillOfCar.Models;

public class CarContext : DbContext
{
    public CarContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
}