using Microsoft.EntityFrameworkCore;

namespace BillOfCar.Models;

public class LogContext : DbContext
{
    public LogContext(DbContextOptions<LogContext> options) : base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        base.OnConfiguring(optionsBuilder);
    }
    
    public DbSet<Log> Logs { get; set; }
    public DbSet<Message> Messages { get; set; }
}