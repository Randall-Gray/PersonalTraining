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
    public class DayWeightsController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public DayWeightsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/DayWeights
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DayWeight>>> GetDayWeights()
        {
            return await _context.DayWeights.ToListAsync();
        }

        // GET: api/DayWeights/GetDayWeightsByClientIdentityValue/5
        [HttpGet("GetDayWeightsByClientIdentityValue/{identityValue}")]
        public async Task<ActionResult<IEnumerable<DayWeight>>> GetDayWeightsByClientIdentityValue(string identityValue)
        {
            return await _context.DayWeights.Include(c => c.Client)
                         .Where(c => c.Client.IdentityUserId == identityValue).ToListAsync();
        }

        // GET: api/DayWeights/GetChartDataByClientIdentityValue/5
        [HttpGet("GetChartDataByClientIdentityValue/{identityValue}")]
        public async Task<ActionResult<IEnumerable<DayWeight>>> GetChartDataByClientIdentityValue(string identityValue)
        {
            return await _context.DayWeights.Where(c => c.Client.IdentityUserId == identityValue).ToListAsync();
        }

        // GET: api/DayWeights/GetDayWeightsByClientId/5
        [HttpGet("GetDayWeightsByClientId/{id}")]
        public async Task<ActionResult<IEnumerable<DayWeight>>> GetDayWeightsByClientId(int id)
        {
            return await _context.DayWeights.Include(c => c.Client)
                         .Where(c => c.Client.ClientId == id).ToListAsync();
        }

        // GET: api/DayWeights/GetChartDataByClientId/5
        [HttpGet("GetChartDataByClientId/{id}")]
        public async Task<ActionResult<IEnumerable<DayWeight>>> GetChartDataByClientId(int id)
        {
            return await _context.DayWeights.Where(c => c.Client.ClientId == id).ToListAsync();
        }

        // GET: api/DayWeights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DayWeight>> GetDayWeight(int id)
        {
            var dayWeight = await _context.DayWeights.FindAsync(id);

            if (dayWeight == null)
            {
                return NotFound();
            }

            return dayWeight;
        }

        // PUT: api/DayWeights/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDayWeight(int id, DayWeight dayWeight)
        {
            if (id != dayWeight.DayWeightId)
            {
                return BadRequest();
            }

            _context.Entry(dayWeight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayWeightExists(id))
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

        // POST: api/DayWeights
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DayWeight>> PostDayWeight(DayWeight dayWeight)
        {
            _context.DayWeights.Add(dayWeight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDayWeight", new { id = dayWeight.DayWeightId }, dayWeight);
        }

        // DELETE: api/DayWeights/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DayWeight>> DeleteDayWeight(int id)
        {
            var dayWeight = await _context.DayWeights.FindAsync(id);
            if (dayWeight == null)
            {
                return NotFound();
            }

            _context.DayWeights.Remove(dayWeight);
            await _context.SaveChangesAsync();

            return dayWeight;
        }

        private bool DayWeightExists(int id)
        {
            return _context.DayWeights.Any(e => e.DayWeightId == id);
        }
    }
}
