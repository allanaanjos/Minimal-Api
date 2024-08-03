namespace Api.Domain.ViewModels
{
    public class NotificationViewModel
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

    }
}