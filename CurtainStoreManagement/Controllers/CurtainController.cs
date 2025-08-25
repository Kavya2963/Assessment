using CurtainStoreManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CurtainStoreManagement.Data;

namespace CurtainStoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurtainsController : ControllerBase
    {
        private readonly AppDBContext _context;
        public CurtainsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Curtains
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curtain>>> GetCurtains()
        {
            return await _context.curtains.ToListAsync();
        }

        // GET: api/Curtains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curtain>> GetCurtain(int id)
        {
            var curtain = await _context.curtains.FindAsync(id);

            if (curtain == null)
            {
                return NotFound();
            }

            return curtain;
        }

        // POST: api/Curtains
        [HttpPost]
        public async Task<ActionResult<Curtain>> PostCurtain(Curtain curtain)
        {
            _context.curtains.Add(curtain);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurtain), new { id = curtain.CurtainID }, curtain);
        }

        // PUT: api/Curtains/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurtain(int id, Curtain curtain)
        {
            if (id != curtain.CurtainID)
            {
                return BadRequest();
            }
           
            _context.Entry(curtain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurtainExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Curtains/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurtain(int id)
        {
            var curtain = await _context.curtains.FindAsync(id);
            if (curtain == null)
            {
                return NotFound();
            }

            _context.curtains.Remove(curtain);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CurtainExists(int id)
        {
            return _context.curtains.Any(e => e.CurtainID == id);
        }
    }
}
