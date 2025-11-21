using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crates.Domain
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation Property: A tag can be on many albums
        public List<Album> Albums { get; set; } = new();
    }
}
