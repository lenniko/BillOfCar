using BillOfCar.Helpers;
using BillOfCar.Interfaces;
using BillOfCar.Manager;
using BillOfCar.Models;
using BillOfCar.Services;
using CSRedis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("CarMysqlString");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    option.UseMySql(connectionString, serverVersion);
});
builder.Services.AddDbContext<LogContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("LogMysqlString");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    option.UseMySql(connectionString, serverVersion);
});
builder.Services.AddDbContextFactory<LogContext>(option =>
{
    var connectionString = builder.Configuration.GetConnectionString("LogMysqlString");
    var serverVersion = ServerVersion.AutoDetect(connectionString);
    option.UseMySql(connectionString, serverVersion);
}, ServiceLifetime.Scoped);
builder.Services.AddHttpContextAccessor();
builder.Services.AddModules(configuration);
builder.Services.AddScoped<IModuleServiceHelper, ModuleServiceHelper>();
builder.Services.AddScoped<IEventManager, EventManager>();
builder.Services.AddSingleton(builder.Services);
var carContext = builder.Services.BuildServiceProvider().GetRequiredService<CarContext>();
ConfigHelper.Init(carContext);
RedisHelper.Initialization(new CSRedisClient("127.0.0.1:6379, defaultDatabase=0,poolsize=500,ssl=false,writerBuffer=10240"));
// 服务
var contentFactory = builder.Services.BuildServiceProvider().GetRequiredService<IDbContextFactory<LogContext>>();
ProcessManager.contentFactory = contentFactory;
_ = ProcessManager.GetInstance();
_ = new LogProcess();

var app = builder.Build();
app.Urls.Add("http://localhost:8080");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();