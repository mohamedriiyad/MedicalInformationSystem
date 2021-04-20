using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensitivitiesController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public SensitivitiesController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Sensitivities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensitivity>>> GetSensitivities(string id)
        {
            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no operations for this Patient");
            }

            return await _context.Sensitivities
                .Where(d => d.MedicalHistoryId == medicalHistory.Id)
                .ToListAsync();
        }

        // GET: api/Sensitivities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensitivity>> GetSensitivity(int id)
        {
            var sensitivity = await _context.Sensitivities.FindAsync(id);

            if (sensitivity == null)
            {
                return NotFound();
            }

            return sensitivity;
        }

        // PUT: api/Sensitivities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSensitivity(int id, Sensitivity sensitivity)
        {
            if (id != sensitivity.Id)
            {
                return BadRequest();
            }

            _context.Entry(sensitivity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensitivityExists(id))
                {
                    return NotFound();
                }

                return BadRequest("Internal server error");
            }

            return NoContent();
        }

        // POST: api/Sensitivities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sensitivity>> PostSensitivity(string id, Sensitivity sensitivity)
        {
            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no diseases for this Patient");
            }

            sensitivity.MedicalHistoryId = medicalHistory.Id;
            _context.Sensitivities.Add(sensitivity);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Sensitivities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSensitivity(int id)
        {
            var sensitivity = await _context.Sensitivities.FindAsync(id);
            if (sensitivity == null)
            {
                return NotFound();
            }

            _context.Sensitivities.Remove(sensitivity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensitivityExists(int id)
        {
            return _context.Sensitivities.Any(e => e.Id == id);
        }
    }
}
