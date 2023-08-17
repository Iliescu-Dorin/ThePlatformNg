using Notification.Domain.Enums;
using Notification.Domain.ValueObjects.Email;

namespace Notification.Application.Interfaces;

public interface IEmailNotification
{
    Task SendEmailAsync(EmailMessage message, EmailTemplates template);
}
