using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTencina.Data;
using PruebaTencina.Models;

namespace PruebaTencina.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BackendGifsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BackendGifsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BusquedaGif>>> GetAll()
        {
            return await _context.BusquedaGifs.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BusquedaGif>> GetById(int id)
        {
            var item = await _context.BusquedaGifs.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<BusquedaGif>> Create(BusquedaGif gif)
        {
            _context.BusquedaGifs.Add(gif);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = gif.Id }, gif);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BusquedaGif gif)
        {
            if (id != gif.Id) return BadRequest();

            _context.Entry(gif).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gif = await _context.BusquedaGifs.FindAsync(id);
            if (gif == null) return NotFound();

            _context.BusquedaGifs.Remove(gif);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
