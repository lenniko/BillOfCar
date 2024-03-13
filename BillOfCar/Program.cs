using BillOfCar.Models;
using BillOfCar.Services;
using CSRedis;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IUserService, UserService>();
RedisHelper.Initialization(new CSRedisClient("127.0.0.1:6379, defaultDatabase=0,poolsize=500,ssl=false,writerBuffer=10240"));

var app = builder.Build();
// app.Urls.Add("http://localhost:5000");
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