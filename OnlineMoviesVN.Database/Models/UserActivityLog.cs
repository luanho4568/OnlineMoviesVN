using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class UserActivityLog
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string? Email { get; set; }
        public string Action { get; set; }

        public string ActivityType { get; set; } = string.Empty; // "Login" hoặc "Logout"

        public DateTime Timestamp { get; set; } = DateTime.Now; // Thời gian thực hiện hoạt động

        public string? IPAddress { get; set; } // Địa chỉ IP 
    }
}
