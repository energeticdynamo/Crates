using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crates.Domain
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? CoverUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Foreign Key for Artist (One-to-Many)
        public int ArtistId { get; set; }
        public Artist? Artist { get; set; }

        // Many-to-Many Relationships
        // EF Core will automatically create the "AlbumGenres" and "AlbumTags" join tables for you
        public List<Genre> Genres { get; set; } = new();
        public List<Tag> Tags { get; set; } = new();
    }
}
