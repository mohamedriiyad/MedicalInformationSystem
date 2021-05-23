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
    public class DiseasesController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public DiseasesController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Diseases
        [HttpGet]
        [Route("api/Diseases/GetDiseases/{id}")]
        public async Task<ActionResult<IEnumerable<Disease>>> GetDiseases(string id)
        {
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no diseases for this Patient");
            }

            return await _context.Diseases
                .Where(d=>d.MedicalHistoryId == medicalHistory.Id)
                .ToListAsync();
        }

        // GET: api/Diseases/5
        [HttpGet]
        [Route("api/Diseases/GetDisease/{id}")]
        public async Task<ActionResult<Disease>> GetDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);

            if (disease == null)
            {
                return NotFound();
            }

            return disease;
        }

        // PUT: api/Diseases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/Diseases/PutDisease/{id}")]
        public async Task<IActionResult> PutDisease(int id, Disease disease)
        {
            if (id != disease.Id)
            {
                return BadRequest();
            }

            _context.Entry(disease).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiseaseExists(id))
                {
                    return NotFound();
                }

                return BadRequest("Internal server error");
            }

            return NoContent();
        }

        // POST: api/Diseases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/Diseases/PostDisease/{id}")]
        public async Task<ActionResult<Disease>> PostDisease(string id, Disease disease)
        {
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no medical history for this Patient");
            }

            disease.MedicalHistoryId = medicalHistory.Id;

            _context.Diseases.Add(disease);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Diseases/5
        [HttpDelete]
        [Route("api/Diseases/DeleteDisease/{id}")]
        public async Task<IActionResult> DeleteDisease(int id)
        {
            var disease = await _context.Diseases.FindAsync(id);
            if (disease == null)
            {
                return NotFound();
            }

            _context.Diseases.Remove(disease);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiseaseExists(int id)
        {
            return _context.Diseases.Any(e => e.Id == id);
        }
    }
}
