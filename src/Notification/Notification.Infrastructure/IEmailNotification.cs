using Notification.Domain.ValueObjects.Email;

namespace Notification.Infrastructure;
public interface IEmailNotification
{
    Task SendEmailAsync(EmailMessage message, EmailTemplates template);
}
