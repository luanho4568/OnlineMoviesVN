using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class UsersPermission
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Code { get; set; }
        public string? Name { get; set; }

    }
}
