namespace BillOfCar.Models;

public class BaseMessage
{
    public string MessageName { get; set; }

    public BaseMessage(string messageName)
    {
        MessageName = messageName;
    }
}

public class EmailMessage : BaseMessage
{
    public string Email { get; set; }
    public string Msg { get; set; }
    public EmailMessage(string messageName = "EmailMessage") : base(messageName)
    {
    }
}