using System.ComponentModel.DataAnnotations;

namespace OnlineMoviesVN.Database.Models
{
    public class Movies
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? OriginName { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? PosterUrl { get; set; }
        public string? ThumbUrl { get; set; }
        public bool? IsCopyright { get; set; }
        public bool? SubDocquyen { get; set; }
        public bool? ChieuRap { get; set; }
        public string? TrailerUrl { get; set; }
        public string? Time { get; set; }
        public string? EpisodeCurrent { get; set; }
        public string? EpisodeTotal { get; set; }
        public string? Quality { get; set; }
        public string? Lang { get; set; }
        public string? Notify { get; set; }
        public string? ShowTimes { get; set; }
        public int? Year { get; set; }
        public int? View { get; set; }
        public List<string>? Actors { get; set; }
        public List<string>? Category { get; set; }
        public List<string>? Country { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
