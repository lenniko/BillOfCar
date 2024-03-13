using BillOfCar.Consts;
using BillOfCar.Models;
using Microsoft.EntityFrameworkCore;

namespace BillOfCar.Services;

public class UserService : IUserService
{
    private CarContext _context;
    public UserService(CarContext context)
    {
        _context = context;
    }
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