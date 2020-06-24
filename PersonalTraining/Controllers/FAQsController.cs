using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalTraining.Data;
using PersonalTraining.Models;

namespace PersonalTraining.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public FAQsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/FAQs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetFAQs()
        {
            return await _context.FAQs.ToListAsync();
        }

        // GET: api/FAQs/GetGeneralFAQs
        [HttpGet("GetGeneralFAQs")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetGeneralFAQs()
        {
            return await _context.FAQs.Where(f => f.General == true).ToListAsync();
        }

        // GET: api/FAQs/GetClientFAQs
        [HttpGet("GetClientFAQs")]
        public async Task<ActionResult<IEnumerable<FAQ>>> GetClientFAQs()
        {
            return await _context.FAQs.Where(f => f.General == false).ToListAsync();
        }

        // GET: api/FAQs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FAQ>> GetFAQ(int id)
        {
            var FAQ = await _context.FAQs.FindAsync(id);

            if (FAQ == null)
            {
                return NotFound();
            }

            return FAQ;
        }

        // PUT: api/FAQs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFAQ(int id, FAQ fAQ)
        {
            if (id != fAQ.FAQId)
            {
                return BadRequest();
            }

            _context.Entry(fAQ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FAQExists(id))
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

        // POST: api/FAQs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FAQ>> PostFAQ(FAQ fAQ)
        {
            _context.FAQs.Add(fAQ);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFAQ", new { id = fAQ.FAQId }, fAQ);
        }

        // DELETE: api/FAQs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FAQ>> DeleteFAQ(int id)
        {
            var fAQ = await _context.FAQs.FindAsync(id);
            if (fAQ == null)
            {
                return NotFound();
            }

            _context.FAQs.Remove(fAQ);
            await _context.SaveChangesAsync();

            return fAQ;
        }

        private bool FAQExists(int id)
        {
            return _context.FAQs.Any(e => e.FAQId == id);
        }
    }
}
