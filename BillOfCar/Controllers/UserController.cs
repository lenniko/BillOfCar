using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;
using BillOfCar.Helpers;
using BillOfCar.Models;
using BillOfCar.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BillOfCar.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    protected readonly CarContext _context;
    protected readonly IHttpContextAccessor _httpContextAccessor;
    public UserController(CarContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    public async Task<IActionResult> RegByEmail([FromForm] string email, [FromForm] string password)
    {
        var sw = new Stopwatch();
        sw.Start();
        if (!StringHelper.IsValidEmail(email))
        {
            return Ok("邮件格式不符合要求!");
        }

        if (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            return Ok("密码长度不能低于8位");
        }
        var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email));
        if (user != null)
        {
            return Ok("该邮箱已注册,请直接登录！");
        }

        user = new User()
        {
            UserName = "MoMo",
            Email = email,
            Password = StringHelper.MD5(password),
            Avatar = ImageHelper.GenerateAvatar(),
            CreatedAt = DatetimeHelper.Now,
            UpdatedAt = DatetimeHelper.Now,
            IsDeleted = false
        };
        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync();
        if (result > 0)
        {
            Console.WriteLine("保存成功");
        }

        await RedisHelper.SetAsync(user.Id.ToString(), email);
        if (RedisHelper.Exists(user.Id.ToString()))
        {
            await RedisHelper.ExpireAsync(user.Id.ToString(), 300);
        }
        sw.Stop();
        Console.WriteLine($"u {user.Id} 注册成功 耗时 {sw.Elapsed.TotalMilliseconds} ms");
        return Ok($"恭喜您注册成功,请直接登录吧!{JsonConvert.SerializeObject(user)}");
    }

    [HttpPost]
    public async Task<IActionResult> Test([FromForm] int start, [FromForm] int limit)
    {
         var users = _context.Users.Where(u => u.Id >= start && u.Id < start + limit);
         Console.WriteLine(users.Count());
        
         return Ok(users);
    }
    [HttpPost]
    public async Task<IActionResult> SignByEmail([FromForm] string email, [FromForm] string password)
    {
        if (!StringHelper.IsValidEmail(email))
        {
            return Ok("邮件格式不符合要求!");
        }

        if (!StringHelper.IsValidPassword(password))
        {
            return Ok("密码不符合格式要求");
        }

        var user = _context.Users.FirstOrDefault(u => u.Email.Equals(email));
        if (user == null)
        {
            return Ok("用户尚未注册!");
        }

        if (!user.Password.Equals(StringHelper.MD5(password)))
        {
            return Ok($"密码错误, 您还可以尝试3次!");
        }

        var token = JwtHelper.GenerateJwtToken(user.Id);
        _httpContextAccessor.HttpContext.Response.Headers.Add("Authorization", "Bearer " + token);
        return Ok("登录成功!");
    }

    [HttpPost]
    public async Task<IActionResult> SignByTelephone([FromForm] string telephone, [FromForm] string password)
    {
        _context.Add(new User()
        {
            UserName = "MoMo",
            PhoneNumber = telephone,
            Password = password,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        });
        await _context.SaveChangesAsync();
        return Ok();
    }
}