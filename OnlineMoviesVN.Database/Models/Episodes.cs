using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMoviesVN.Database.Models
{
    public class Episodes
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? Filename { get; set; }
        public string? LinkEmbed { get; set; }
        public string? LinkM3U8 { get; set; }
        public Guid? MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        [ValidateNever]
        public Movies? Movie { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
