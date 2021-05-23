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
    public class MedicinesController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public MedicinesController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicines
        [HttpGet]
        [Route("api/Medicines/GetMedicines/{id}")]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines(string id)
        {
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no medical history for this Patient");
            }

            return await _context.Medicines
                .Where(d => d.MedicalHistoryId == medicalHistory.Id)
                .ToListAsync();
        }

        // GET: api/Medicines/5
        [HttpGet]
        [Route("api/Medicines/GetMedicine/{id}")]
        public async Task<ActionResult<Medicine>> GetMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);

            if (medicine == null)
            {
                return NotFound();
            }

            return medicine;
        }

        // PUT: api/Medicines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/Medicines/PutMedicine/{id}")]
        public async Task<IActionResult> PutMedicine(int id, Medicine medicine)
        {
            if (id != medicine.Id)
            {
                return BadRequest();
            }

            _context.Entry(medicine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicineExists(id))
                {
                    return NotFound();
                }

                return BadRequest("Internal server error");
            }

            return NoContent();
        }

        // POST: api/Medicines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/Medicines/PostMedicine/{id}")]
        public async Task<ActionResult<Medicine>> PostMedicine(string id, Medicine medicine)
        {
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no medical history for this Patient");
            }

            medicine.MedicalHistoryId = medicalHistory.Id;

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Medicines/5
        [HttpDelete]
        [Route("api/Medicines/DeleteMedicine/{id}")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicines.Any(e => e.Id == id);
        }
    }
}
