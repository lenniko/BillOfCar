namespace BillOfCar.Consts;

public enum PacketIds
{
    PacketFactor = 1000,
    //IlnterlCode
    Common_InternalError = 1010,
    // CarService
    Car_AddCar = 1100,
    Car_DeleteCar = 1101,
    Car_UpdateCar = 1102,
    Car_AddItem = 1111,
    Car_DeleteItem = 1112,
    Car_UpdateItem = 1113,

    // UserService
    User_RegisterByEmail = 3100,
    User_BindPhone = 3200,
    User_SendCode = 3201,
    // MailService
    Mail_SendMail = 5100,

}