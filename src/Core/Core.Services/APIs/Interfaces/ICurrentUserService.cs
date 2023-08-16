namespace Core.Services.APIs.Interfaces;
public interface ICurrentUserService
{
    public string UserId { get; }
    bool IsAuthenticated { get; }
    public string IpAddress { get; }
}
