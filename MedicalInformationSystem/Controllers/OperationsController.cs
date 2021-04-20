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
    public class OperationsController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public OperationsController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Operations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operation>>> GetOperations(string id)
        {
            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no operations for this Patient");
            }

            return await _context.Operations
                .Where(d => d.MedicalHistoryId == medicalHistory.Id)
                .ToListAsync();
        }

        // GET: api/Operations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operation>> GetOperation(int id)
        {
            var operation = await _context.Operations.FindAsync(id);

            if (operation == null)
            {
                return NotFound();
            }

            return operation;
        }

        // PUT: api/Operations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperation(int id, Operation operation)
        {
            if (id != operation.Id)
            {
                return BadRequest();
            }

            _context.Entry(operation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationExists(id))
                {
                    return NotFound();
                }

                return BadRequest("Internal server error");
            }

            return NoContent();
        }

        // POST: api/Operations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Operation>> PostOperation(string id, Operation operation)
        {
            var medicalHistory = _context.MedicalHistory.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no diseases for this Patient");
            }

            operation.MedicalHistoryId = medicalHistory.Id;
            _context.Operations.Add(operation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Operations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperation(int id)
        {
            var operation = await _context.Operations.FindAsync(id);
            if (operation == null)
            {
                return NotFound();
            }

            _context.Operations.Remove(operation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OperationExists(int id)
        {
            return _context.Operations.Any(e => e.Id == id);
        }
    }
}
