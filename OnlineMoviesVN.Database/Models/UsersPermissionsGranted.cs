using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class UsersPermissionsGranted
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? UserRole { get; set; }

        public int? RoleLevel { get; set; }

        public string? PermissionCode { get; set; }

        public string? PermissionName { get; set; }

        public bool? Active { get; set; }
    }
}
