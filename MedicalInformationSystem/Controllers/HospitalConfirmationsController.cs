using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MedicalInformationSystem.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalConfirmationsController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public HospitalConfirmationsController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/HospitalConfirmations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalConfirmation>>> GetHospitalConfirmations()
        {
            return await _context.HospitalConfirmations
                .Include(h=>h.Files)
                .ToListAsync();
        }

        // GET: api/HospitalConfirmations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalConfirmation>> GetHospitalConfirmation(int id)
        {
            var hospitalConfirmation = await _context.HospitalConfirmations
                .Include(h=>h.Files)
                .FirstOrDefaultAsync(h=>h.Id == id);

            if (hospitalConfirmation == null)
            {
                return NotFound();
            }

            return hospitalConfirmation;
        }

        // PUT: api/HospitalConfirmations/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutHospitalConfirmation(int id, HospitalConfirmation hospitalConfirmation)
        //{
        //    if (id != hospitalConfirmation.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(hospitalConfirmation).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!HospitalConfirmationExists(id))
        //        {
        //            return NotFound();
        //        }

        //        throw;
        //    }

        //    return NoContent();
        //}

        // POST: api/HospitalConfirmations
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost,DisableRequestSizeLimit]
        public async Task<ActionResult<HospitalConfirmation>> PostHospitalConfirmation([FromForm] HospitalUploadModel model)
        {
            var formFiles = model.Files.ToList();
            var hospitalFiles = new List<HospitalFile>();
            //var files = Request.Form.Files.ToList();
            var folderName = Path.Combine("Resources", "Files");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            List<string> files;

            try
            {
                if (formFiles.Any(f => f.Length == 0))
                    return BadRequest("There is no files or empty files");

                files = FileHelper.UploadAll(formFiles, pathToSave);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            var hospitalConfirmation = new HospitalConfirmation
            {
                Name = model.Name,
                Email = model.Email,
                Location = model.Location,
                Password = model.Password
            };
            _context.HospitalConfirmations.Add(hospitalConfirmation);
            await _context.SaveChangesAsync();

            foreach (var file in files)
            {
                hospitalFiles.Add(new HospitalFile { FilePath = file, HospitalConfirmationId = hospitalConfirmation.Id });
            }

            _context.HospitalFiles.AddRange(hospitalFiles);
            _context.SaveChanges();
            


            return Ok("All files are UPLOADED SUCCESSFULLY!");

        }

        // DELETE: api/HospitalConfirmations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHospitalConfirmation(int id)
        {
            var hospitalConfirmation = await _context.HospitalConfirmations
                .Include(h=>h.Files)
                .FirstOrDefaultAsync(h=>h.Id == id);
            if (hospitalConfirmation == null)
            {
                return NotFound();
            }

            _context.HospitalConfirmations.Remove(hospitalConfirmation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool HospitalConfirmationExists(int id)
        //{
        //    return _context.HospitalConfirmations.Any(e => e.Id == id);
        //}
    }
}
