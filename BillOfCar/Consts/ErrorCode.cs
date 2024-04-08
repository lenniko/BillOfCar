namespace BillOfCar.Consts;

public class ErrorCode
{
    public const int Success = 0;

    public const int General_ErrorParam = 100;
    public const int General_ErrorConfig = 101;

    public const int User_ExistedEmail = 1001;
    public const int User_ErrorPassword = 1002;
    public const int User_ErrorCode = 1003;
    public const int User_CodeExceedLimit = 1004;
    public const int User_CodeNotExpired = 1005;
}