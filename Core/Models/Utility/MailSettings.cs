namespace Core.Models.Utility
{
    // Cấu hình dịch vụ gửi mail, giá trị Inject từ appsettings.json
    public class MailSettings
    {
        public string? Mail { get; set; }
        public string? DisplayName { get; set; }
        public string? Password { get; set; }
        public string? Host { get; set; }
        public int Port { get; set; }

    }
}
