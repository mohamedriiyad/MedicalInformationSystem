using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalInformationSystem.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly MedicalSystemDbContext _context;

        public TestsController(MedicalSystemDbContext context)
        {
            _context = context;
        }

        // GET: api/Tests
        [HttpGet]
        [Route("api/Tests/GetTests/{id}")]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests(string id)
        {
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
            {
                return BadRequest("There is no medicines for this Patient");
            }


            return await _context.Tests
                .Where(d => d.MedicalHistoryId == medicalHistory.Id)
                .ToListAsync();
        }

        // GET: api/Tests/5
        [HttpGet]
        [Route("api/Tests/GetTest/{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }

        // PUT: api/Tests/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("api/Tests/PutTest/{id}")]
        public async Task<IActionResult> PutTest(int id, Test test)
        {
            if (id != test.Id)
            {
                return BadRequest();
            }

            _context.Entry(test).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestExists(id))
                {
                    return NotFound();
                }

                return BadRequest("Internal server error");
            }

            return NoContent();
        }

        // POST: api/Tests
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("api/Tests/PostTest/{id}")]
        public async Task<ActionResult<Test>> PostTest(string id, [FromForm]TestUploadModel model)
        {
            var formFiles = model.Files.ToList();
            //var hospitalFiles = new List<HospitalFile>();
            //var files = Request.Form.Files.ToList();
            var pathToSave = @"images/tests";
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
            var medicalHistory = _context.MedicalHistories.FirstOrDefault(m => m.ApplicationUserId == id);
            if (medicalHistory == null)
                return BadRequest("There is no medical history for this Patient");

            var test = new Test
            {
                Date = model.Date,
                Image = files.FirstOrDefault(),
                MedicalHistoryId = medicalHistory.Id,
                Name = model.Name
            };

            test.MedicalHistoryId = medicalHistory.Id;
            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Tests/5
        [HttpDelete]
        [Route("api/Tests/DeleteTest/{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestExists(int id)
        {
            return _context.Tests.Any(e => e.Id == id);
        }
    }
}
