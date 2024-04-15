namespace BillOfCar.Interfaces;

public interface IProcess
{
    void Init();
    Task Update();
    void OnDestroy();
    void Destroy();
}