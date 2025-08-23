namespace AWSsnsApi.Services
{
    public interface INotificationService
    {
        Task SendUploadNotification(string topic, string message);
        Task SendUploadNotification(string message);
        Task RegisterSubscirption(string email);
        Task RegisterSubscirption(string topic, string email);
    }
}
