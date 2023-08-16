namespace Notification.Domain.ValueObjects.Email;
public class EmailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public object Model { get; set; }
}
