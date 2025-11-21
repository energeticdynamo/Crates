using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crates.Domain
{
    /// <summary>
    /// Represents an artist.
    /// </summary>
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }

        // Navigation Property: An artist has many albums
        public List<Album> Albums { get; set; } = new();
    }
}
