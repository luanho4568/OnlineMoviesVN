using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [MinLength(6, ErrorMessage = "Họ tên ít nhất 6 ký tự")]
        public string? FullName { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? Gender { get; set; }
        [StringLength(11, MinimumLength = 10, ErrorMessage = "Vui lòng nhập đúng số điện thoại")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Số điện thoại chỉ được phép nhập số.")]
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string? AccountType { get; set; } = "local".ToUpper();
        public string Role { get; set; } = "member".ToUpper();

        public string? Avatar { get; set; }

        public bool? IsActive { get; set; } = true;
        public bool? IsStatus { get; set; } = false;

        public string? VerificationCode { get; set; } // Mã OTP hoặc mã xác thực
        public DateTime? VerificationCodeExpired { get; set; } // Hạn mã OTP

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastLogout { get; set; }

    }
}
