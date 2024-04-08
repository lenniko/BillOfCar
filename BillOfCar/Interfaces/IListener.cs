namespace BillOfCar.Models;

public interface IListener
{
    void Init();
    void AddListener();
    void RemoveListener();
}