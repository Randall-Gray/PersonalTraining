using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPFWebsitMVC.Data;
using SPFWebsitMVC.Models;

namespace SPFWebsitMVC.Controllers
{
    public class BroadcastMessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BroadcastMessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BroadcastMessages
        public async Task<IActionResult> Index()
        {
            return View(await _context.BroadcastMessage.ToListAsync());
        }

        // GET: BroadcastMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var broadcastMessage = await _context.BroadcastMessage
                .FirstOrDefaultAsync(m => m.BroadcastMessageId == id);
            if (broadcastMessage == null)
            {
                return NotFound();
            }

            return View(broadcastMessage);
        }

        // GET: BroadcastMessages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BroadcastMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BroadcastMessageId,Message,DatePosted,NumDays,PosterName")] BroadcastMessage broadcastMessage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(broadcastMessage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(broadcastMessage);
        }

        // GET: BroadcastMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var broadcastMessage = await _context.BroadcastMessage.FindAsync(id);
            if (broadcastMessage == null)
            {
                return NotFound();
            }
            return View(broadcastMessage);
        }

        // POST: BroadcastMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BroadcastMessageId,Message,DatePosted,NumDays,PosterName")] BroadcastMessage broadcastMessage)
        {
            if (id != broadcastMessage.BroadcastMessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(broadcastMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BroadcastMessageExists(broadcastMessage.BroadcastMessageId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(broadcastMessage);
        }

        // GET: BroadcastMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var broadcastMessage = await _context.BroadcastMessage
                .FirstOrDefaultAsync(m => m.BroadcastMessageId == id);
            if (broadcastMessage == null)
            {
                return NotFound();
            }

            return View(broadcastMessage);
        }

        // POST: BroadcastMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var broadcastMessage = await _context.BroadcastMessage.FindAsync(id);
            _context.BroadcastMessage.Remove(broadcastMessage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BroadcastMessageExists(int id)
        {
            return _context.BroadcastMessage.Any(e => e.BroadcastMessageId == id);
        }
    }
}
