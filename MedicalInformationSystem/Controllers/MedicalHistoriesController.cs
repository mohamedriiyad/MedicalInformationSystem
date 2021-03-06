using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalInformationSystem.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
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
        [Route("api/MedicalHistories/GetMedicalHistory")]
        public async Task<IEnumerable<MedicalHistoryViewModel>> GetMedicalHistory()
        {
            var medicalHistories = await _context.MedicalHistories
                .Include(m => m.ApplicationUser)
                .Include(m => m.Operations)
                .Include(m => m.Diseases)
                .Include(m => m.Sensitivities)
                .Include(m => m.Medicines)
                .Include(m => m.Tests)
                .Select(a => new MedicalHistoryViewModel
                {
                    Id = a.Id,
                    ApplicationUserId = a.ApplicationUser.Id,
                    PatientSSN = a.ApplicationUser.PatientSSN,
                    BloodType = a.BloodType,
                    Operations = a.Operations,
                    Sensitivities = a.Sensitivities,
                    Diseases = a.Diseases,
                    FullName = a.ApplicationUser.FullName,
                    Tests = a.Tests,
                    Medicines = a.Medicines
                })
                .ToListAsync();

            return medicalHistories;
        }

        // GET: api/MedicalHistories/5
        [HttpGet]
        [Route("api/MedicalHistories/GetMedicalHistory/{id}")]
        public async Task<ActionResult<MedicalHistoryViewModel>> GetMedicalHistory(string id)
        {
            if (!MedicalHistoryUserExists(id))
                return NotFound("This user doesn't have any medical history yet.");

            var medicalHistoryInDb = await _context.MedicalHistories
                .Include(m => m.ApplicationUser)
                .Include(m => m.Operations)
                .Include(m => m.Diseases)
                .Include(m => m.Sensitivities)
                .Include(m => m.Tests)
                .Include(m => m.Medicines)
                .FirstOrDefaultAsync(m => m.ApplicationUserId == id);
            
            var medicalHistory = new MedicalHistoryViewModel
            {
                Id = medicalHistoryInDb.Id,
                PatientSSN = medicalHistoryInDb.ApplicationUser.PatientSSN,
                FullName = medicalHistoryInDb.ApplicationUser.FullName,
                BloodType = medicalHistoryInDb.BloodType,
                Diseases = medicalHistoryInDb.Diseases,
                Sensitivities = medicalHistoryInDb.Sensitivities,
                Operations = medicalHistoryInDb.Operations,
                Tests = medicalHistoryInDb.Tests,
                Medicines = medicalHistoryInDb.Medicines,
                ApplicationUserId = medicalHistoryInDb.ApplicationUserId
            };

            return medicalHistory;
        }

        [HttpPut]
        [Route("api/MedicalHistories/PutMedicalHistory/{id}")]
        public async Task<IActionResult> PutMedicalHistory(string id, MedicalHistoryInput medicalHistory)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
                return BadRequest("This id has no user!!");

            if (!MedicalHistoryUserExists(id))
                return NotFound("This user doesn't have any medical history yet.");

            var medicalHistoryInDb =
                await _context.MedicalHistories.FirstOrDefaultAsync(m => m.ApplicationUserId == id);
            medicalHistoryInDb.BloodType = medicalHistory.BloodType;
            _context.MedicalHistories.Update(medicalHistoryInDb);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicalHistoryUserExists(id))
                    return NotFound();
                return BadRequest("There is something wrong!!");
            }

            return NoContent();
        }

        // POST: api/MedicalHistories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/MedicalHistories/PostMedicalHistory/{id}")]
        public async Task<ActionResult<MedicalHistory>> PostMedicalHistory(string id, MedicalHistoryInput input)
        {
            var patient = _context.Patients.Find(id);

            if (patient == null)
                return BadRequest("This id has no user!!");
            if (MedicalHistoryUserExists(id))
                return BadRequest("This user already have a medical history!!");

            var medicalHistory = new MedicalHistory
            {
                BloodType = input.BloodType,
                ApplicationUserId = id
            };
            _context.MedicalHistories.Add(medicalHistory);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/MedicalHistories/5
        [HttpDelete]
        [Route("api/MedicalHistories/DeleteMedicalHistory/{id}")]
        public async Task<IActionResult> DeleteMedicalHistory(string id)
        {
            var medicalHistory = await _context.MedicalHistories.FirstOrDefaultAsync(m=>m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return NotFound();
            }

            _context.MedicalHistories.Remove(medicalHistory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicalHistoryUserExists(string id)
        {
            return _context.MedicalHistories.Any(e => e.ApplicationUserId == id);
        }
    }
}
