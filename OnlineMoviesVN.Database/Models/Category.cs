using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Slug { get; set; }
    }
}
