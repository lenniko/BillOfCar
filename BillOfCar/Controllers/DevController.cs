using BillOfCar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BillOfCar.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class DevController : ControllerBase
{
    private readonly CarContext _context;
    private readonly LogContext _logContext;
    private const string Secret = "qwekcmvsdoafmdaf";
    public DevController(CarContext context, LogContext logContext)
    {
        _context = context;
        _logContext = logContext;
    }
    [HttpPost]
    public async Task<IActionResult> Log([FromQuery]string secret, [FromBody] LogQuery query)
    {
        if (!secret.Equals(Secret))
        {
            return Ok("密钥错误");
        }

        var last = _logContext.Logs.OrderByDescending(log => log.Id).Last();
        var index = query.Index == -1 ? last.Id : query.Index;
        var queryable = _logContext.Logs.Where(log => log.Id <= index && log.Id > index - query.Limit).ToList();
        return Ok(queryable);
    }
}