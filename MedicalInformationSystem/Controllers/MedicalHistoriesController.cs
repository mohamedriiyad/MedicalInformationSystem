using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalHistoriesController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public MedicalHistoriesController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/MedicalHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MedicalHistory>>> GetMedicalHistory()
        {
            return await _context.MedicalHistory.ToListAsync();
        }

        // GET: api/MedicalHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MedicalHistory>> GetMedicalHistory(string id)
        {
            var medicalHistory = await _context.MedicalHistory
                .Include(m => m.ApplicationUser)
                .Include(m => m.Operations)
                .Include(m => m.Diseases)
                .Include(m => m.Sensitivities)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return NotFound("This user doesn't have any medical history yet.");
            }

            return medicalHistory;
        }

        // PUT: api/MedicalHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedicalHistory(string id, MedicalHistory medicalHistory)
        {
            if (id != medicalHistory.ApplicationUserId)
            {
                return BadRequest();
            }
            //var _medicalHistory = new MedicalHistory
            //{
            //    BloodType = medicalHistory.BloodType,
            //    ApplicationUserId = medicalHistory.ApplicationUserId
            //};
            _context.Entry(medicalHistory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryUserExists(id))
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

        // POST: api/MedicalHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MedicalHistory>> PostMedicalHistory(MedicalHistoryViewModel _medicalHistory)
        {
            var medicalHistory = new MedicalHistory
            {
                BloodType = _medicalHistory.BloodType,
                ApplicationUserId = _medicalHistory.ApplicationUserId
            };
            _context.MedicalHistory.Add(medicalHistory);
            _context.SaveChanges();

            foreach (var operation in _medicalHistory.Operations)
            {
                operation.MedicalHistoryId = medicalHistory.Id;
            }

            foreach (var disease in _medicalHistory.Diseases)
            {
                disease.MedicalHistoryId = medicalHistory.Id;
            }

            foreach (var sensitivity in _medicalHistory.Sensitivities)
            {
                sensitivity.MedicalHistoryId = medicalHistory.Id;
            }
            
            _context.Operations.AddRange(_medicalHistory.Operations);
            _context.Sensitivities.AddRange(_medicalHistory.Sensitivities);
            _context.Diseases.AddRange(_medicalHistory.Diseases);

            _context.SaveChanges();
            return CreatedAtAction("GetMedicalHistory", new { id = medicalHistory.Id }, medicalHistory);
        }

        // DELETE: api/MedicalHistories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedicalHistory(int id)
        {
            var medicalHistory = await _context.MedicalHistory.FindAsync(id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            _context.MedicalHistory.Remove(medicalHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalHistoryUserExists(string id)
        {
            return _context.MedicalHistory.Any(e => e.ApplicationUserId == id);
        }
    }
}
