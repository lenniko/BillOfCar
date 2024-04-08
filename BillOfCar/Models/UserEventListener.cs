using BillOfCar.Consts;

namespace BillOfCar.Models;

public class UserEventListener : IListener
{
    protected EventManager _eventManager;
    public UserEventListener(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    public void Init()
    {
        Console.WriteLine("UserEventListener 开始监听");
        AddListener();
    }

    public void AddListener()
    {
        _eventManager.AddListener<User>(EventType.User_Register, OnUserRegister);
        _eventManager.AddListener<User>(EventType.User_Update, OnUserUpdate);
        _eventManager.AddListener<User>(EventType.User_Delete, OnUserDelete);
    }

    private void OnUserDelete(User user)
    {
        Console.WriteLine("User Register" + user.Id);
    }

    private void OnUserUpdate(User user)
    {
        Console.WriteLine("User Register" + user.Id);
    }

    private void OnUserRegister(User user)
    {
        Console.WriteLine("User Register" + user.Id);
    }

    public void RemoveListener()
    {
        _eventManager.RemoveListener<User>(EventType.User_Register, OnUserRegister);
        _eventManager.RemoveListener<User>(EventType.User_Update, OnUserUpdate);
        _eventManager.RemoveListener<User>(EventType.User_Delete, OnUserDelete);
    }
}