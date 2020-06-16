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
    public class ExerciseClassesController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ExerciseClassesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: api/ExerciseClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseClass>>> GetExerciseClasses()
        {
            return await _context.ExerciseClasses.ToListAsync();
        }

        // GET: api/ExerciseClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExerciseClass>> GetExerciseClass(int id)
        {
            var exerciseClass = await _context.ExerciseClasses.FindAsync(id);

            if (exerciseClass == null)
            {
                return NotFound();
            }

            return exerciseClass;
        }

        // PUT: api/ExerciseClasses/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExerciseClass(int id, ExerciseClass exerciseClass)
        {
            if (id != exerciseClass.ExerciseClassId)
            {
                return BadRequest();
            }

            _context.Entry(exerciseClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseClassExists(id))
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

        // POST: api/ExerciseClasses
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ExerciseClass>> PostExerciseClass(ExerciseClass exerciseClass)
        {
            _context.ExerciseClasses.Add(exerciseClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExerciseClass", new { id = exerciseClass.ExerciseClassId }, exerciseClass);
        }

        // DELETE: api/ExerciseClasses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ExerciseClass>> DeleteExerciseClass(int id)
        {
            var exerciseClass = await _context.ExerciseClasses.FindAsync(id);
            if (exerciseClass == null)
            {
                return NotFound();
            }

            _context.ExerciseClasses.Remove(exerciseClass);
            await _context.SaveChangesAsync();

            return exerciseClass;
        }

        private bool ExerciseClassExists(int id)
        {
            return _context.ExerciseClasses.Any(e => e.ExerciseClassId == id);
        }
    }
}
