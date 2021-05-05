using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var medicalHistories = await _context.MedicalHistory
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
            {
                return NotFound("This user doesn't have any medical history yet.");
            }

            var medicalHistoryInDb = await _context.MedicalHistory
                .Include(m => m.ApplicationUser)
                .Include(m => m.Operations)
                .Include(m => m.Diseases)
                .Include(m => m.Sensitivities)
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
                ApplicationUserId = medicalHistoryInDb.ApplicationUserId
            };

            

            return medicalHistory;
        }

        // PUT: api/MedicalHistories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/MedicalHistories/PutMedicalHistory/{id}")]
        public async Task<IActionResult> PutMedicalHistory(string id, MedicalHistoryViewModel medicalHistory)
        {
            if (id != medicalHistory.ApplicationUserId)
            {
                return BadRequest();
            }

            if (!MedicalHistoryUserExists(id))
            {
                return NotFound("This user doesn't have any medical history yet.");
            }

            var medicalHistoryInDb = new MedicalHistory
            {
                Id = medicalHistory.Id,
                BloodType = medicalHistory.BloodType,
                ApplicationUserId = medicalHistory.ApplicationUserId
            };

            _context.Entry(medicalHistoryInDb).State = EntityState.Modified;

            if (medicalHistory.Operations != null)
            {
                var operations = new List<Operation>();
                operations.AddRange(medicalHistory.Operations);
                medicalHistoryInDb.Operations = operations;


                foreach (var operation in medicalHistoryInDb.Operations)
                {
                    _context.Entry(operation).State = EntityState.Modified;
                }
            }

            if (medicalHistory.Sensitivities != null)
            {
                var sensitivities = new List<Sensitivity>();
                sensitivities.AddRange(medicalHistory.Sensitivities);
                medicalHistoryInDb.Sensitivities = sensitivities;

                foreach (var sensitivity in medicalHistoryInDb.Sensitivities)
                {
                    _context.Entry(sensitivity).State = EntityState.Modified;
                }

            }

            if (medicalHistory.Diseases != null)
            {
                var diseases = new List<Disease>();
                diseases.AddRange(medicalHistory.Diseases);
                medicalHistoryInDb.Diseases = diseases;

                foreach (var disease in medicalHistoryInDb.Diseases)
                {
                    _context.Entry(disease).State = EntityState.Modified;
                }

            }

            
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
        [Route("api/MedicalHistories/PostMedicalHistory")]
        public async Task<ActionResult<MedicalHistory>> PostMedicalHistory(MedicalHistoryViewModel medicalHistoryModel)
        {
            var medicalHistory = new MedicalHistory
            {
                BloodType = medicalHistoryModel.BloodType,
                ApplicationUserId = medicalHistoryModel.ApplicationUserId
            };
            _context.MedicalHistory.Add(medicalHistory);
            await _context.SaveChangesAsync();

            foreach (var operation in medicalHistoryModel.Operations)
            {
                operation.MedicalHistoryId = medicalHistory.Id;
            }

            foreach (var disease in medicalHistoryModel.Diseases)
            {
                disease.MedicalHistoryId = medicalHistory.Id;
            }

            foreach (var sensitivity in medicalHistoryModel.Sensitivities)
            {
                sensitivity.MedicalHistoryId = medicalHistory.Id;
            }
            
            _context.Operations.AddRange(medicalHistoryModel.Operations);
            _context.Sensitivities.AddRange(medicalHistoryModel.Sensitivities);
            _context.Diseases.AddRange(medicalHistoryModel.Diseases);

            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/MedicalHistories/5
        [HttpDelete]
        [Route("api/MedicalHistories/DeleteMedicalHistory/{id}")]
        public async Task<IActionResult> DeleteMedicalHistory(string id)
        {
            var medicalHistory = await _context.MedicalHistory.FirstOrDefaultAsync(m=>m.ApplicationUserId == id);
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
