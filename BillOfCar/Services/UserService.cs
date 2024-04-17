using BillOfCar.Attributes;
using BillOfCar.ClientApi;
using BillOfCar.ClientApi.UserAction;
using BillOfCar.Consts;
using BillOfCar.Helpers;
using BillOfCar.Interfaces;
using BillOfCar.Manager;
using BillOfCar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Utils = BillOfCar.Helpers.Utils;

namespace BillOfCar.Services;

[Service(3)]
public class UserService : BaseService
{
    private CarContext _context;
    public UserService(CarContext context)
    {
        _context = context;
    }
    [Method((int)PacketIds.User_RegisterByEmail % GamePacket.Factor)]
    public async Task<bool> RegUserByEmailAsync(string email, string password)
    {
        var user = _context.Users.FirstOrDefault(user => user.Email.Equals(email));
        if (user != null)
        {
            return false;
        }

        user = new User()
        {
            UserName = "Moji",
            Email = email,
            Password = password,
            Role = eRole.NormalUser,
            CreatedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    [Method((int)PacketIds.User_SendCode % GamePacket.Factor, false)]
    public async Task<IPacketBody> SendCode(SendCodeRequest request)
    {
        var m = new EmailMessage()
        {
            Email = request.Telephone,
            Msg = "测试消息"
        };
        var message = new Message()
        {
            Type = eMessage.Email,
            UserId = -1,
            UUID = Guid.NewGuid().ToString().ToLower().Replace("-", ""),
            Content = JsonConvert.SerializeObject(m),
            Timestamp = DatetimeHelper.TimeStamp
        };
        MessageProcess.ProduceMsg(message);
        var limit = ConfigHelper.Get("Limit");
        var phone = request.Telephone;
        var key = Redis_Extension.GenerateKey(Redis_Extension.Code, phone);
        var codeLimit = Redis_Extension.GenerateKey(Redis_Extension.CodeLimit, phone);
        if (RedisHelper.Exists(codeLimit))
        {
            await RedisHelper.IncrByAsync(codeLimit, 1);
        }
        else
        {
            await RedisHelper.SetAsync(codeLimit, 1);
            await RedisHelper.ExpireAsync(codeLimit, 60);
        }

        var count = await RedisHelper.GetAsync<int>(codeLimit);
        if (count > 1000)
        {
            Console.WriteLine("[Warning] 用户请求速率异常");
        }
        if (RedisHelper.Exists(key))
        {
            return new SendCodeResponse()
            {
                ErrorCode = ErrorCode.User_CodeNotExpired
            };
        }
        var generatedCode = Utils.GeneratedCode();
        await RedisHelper.SetAsync(key, generatedCode);
        await RedisHelper.ExpireAsync(key, 600);
        
        return new SendCodeResponse()
        {
            ErrorCode = ErrorCode.Success,
            Code = generatedCode
        };
    }
    
    public bool RegUserByTelephone(string telephone)
    {
        throw new NotImplementedException();
    }

    public bool LogUser(User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int UserId)
    {
        throw new NotImplementedException();
    }

    public User GetUser(int UserId)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == UserId);
        return user;
    }

    public void UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public bool ValidUser(User user)
    {
        throw new NotImplementedException();
    }
}