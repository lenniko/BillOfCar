using BillOfCar.Models;
using BillOfCar.Services;
using Microsoft.AspNetCore.Mvc;

namespace BillOfCar.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    protected readonly CarContext _context;
    protected readonly IUserService _userService;
    public UserController(CarContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> RegByEmail([FromForm] string email, [FromForm] string password)
    {
        var result = await _userService.RegUserByEmailAsync(email, password);
        if (result)
        {
            return Ok("Success");
        }
        else
        {
            return Ok("failed");
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetUserInfo([FromForm] int userId)
    {
        var user = _userService.GetUser(userId);
        if (user == null)
        {
            return Ok("Failed");
        }
        else
        {
            return Ok(user);
        }
    }
    [HttpPost]
    public async Task<IActionResult> Sign([FromForm] string telephone, [FromForm] string password)
    {
        _context.Add(new User()
        {
            UserName = "初来乍到",
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