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
    public class BroadcastMessagesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public BroadcastMessagesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/BroadcastMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BroadcastMessage>>> GetBroadcastMessages()
        {
            return await _context.BroadcastMessages.ToListAsync();
        }

        // GET: api/BroadcastMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BroadcastMessage>> GetBroadcastMessage(int id)
        {
            var broadcastMessage = await _context.BroadcastMessages.FindAsync(id);

            if (broadcastMessage == null)
            {
                return NotFound();
            }

            return broadcastMessage;
        }

        // PUT: api/BroadcastMessages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBroadcastMessage(int id, BroadcastMessage broadcastMessage)
        {
            if (id != broadcastMessage.BroadcastMessageId)
            {
                return BadRequest();
            }

            _context.Entry(broadcastMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BroadcastMessageExists(id))
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

        // POST: api/BroadcastMessages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BroadcastMessage>> PostBroadcastMessage(BroadcastMessage broadcastMessage)
        {
            _context.BroadcastMessages.Add(broadcastMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBroadcastMessage", new { id = broadcastMessage.BroadcastMessageId }, broadcastMessage);
        }

        // DELETE: api/BroadcastMessages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BroadcastMessage>> DeleteBroadcastMessage(int id)
        {
            var broadcastMessage = await _context.BroadcastMessages.FindAsync(id);
            if (broadcastMessage == null)
            {
                return NotFound();
            }

            _context.BroadcastMessages.Remove(broadcastMessage);
            await _context.SaveChangesAsync();

            return broadcastMessage;
        }

        private bool BroadcastMessageExists(int id)
        {
            return _context.BroadcastMessages.Any(e => e.BroadcastMessageId == id);
        }
    }
}
