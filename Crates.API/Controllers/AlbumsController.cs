using Crates.Data;
using Crates.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crates.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly CratesContext _context;

        public AlbumsController(CratesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Album>>> GetAlbums()
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Genres)
                .Include(a => a.Tags)
                .ToListAsync();
        }
    }
}
