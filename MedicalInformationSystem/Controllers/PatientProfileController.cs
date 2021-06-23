using MedicalInformationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Controllers
{
    public class PatientProfileController:Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        public PatientProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("api/PatientProfile/GetForAdmin")]
        public string GetForAdmin()
        {
            return "Web method for Admin";
        }

        [HttpGet]
        [Authorize(Roles = "Patient")]
        [Route("api/PatientProfile/GetForPatient")]
        public string GetForPatient()
        {
            return "Web method for patient";
        }

        [HttpGet]
        [Authorize(Roles = "hospital")]
        [Route("api/PatientProfile/GetForHospital")]
        public string GetForHospital()
        {
            return "Web method for hospital";
        }

        [HttpGet]
        [Route("api/PatientProfile/getPatientById/{PatientId}")]
        [Authorize]
        public async Task<IActionResult> GetPatientById(string patientId)
        {
            var user = await _userManager.FindByIdAsync(patientId);
            if (user == null)
            {
                return BadRequest("sorry user not found");
            }
            return Ok(new
            {
                user.FullName,
                user.PhoneNumber,
                user.Gender,
                user.City,
                user.DateOfBirth,
                RealtiveOneName = user.RelativeOneName,
                RealtiveTwoName = user.RelativeTwoName,
                RealtiveOnePhone = user.RelativeOnePhoneNumber,
                RealtiveTwoPhone = user.RelativeTwoPhoneNumber,
            });
        }


        [HttpGet]
        [Route("api/PatientProfile/getPatientBySSN/{PatientSSN}")]
        public async Task<IActionResult> GetPatientBySsn(string patientSsn)
        {
            var user = await _userManager.Users.FirstAsync(e => e.PatientSSN == patientSsn);
            if (user == null)
            {
                return BadRequest("sorry user not found");
            }
            return Ok(new
            {
                user.Id,
                user.FullName,
                user.PhoneNumber,
                user.DateOfBirth,
                RealtiveOneName = user.RelativeOneName,
                RealtiveTwoName = user.RelativeTwoName,
                RealtiveOnePhone = user.RelativeOnePhoneNumber,
                RealtiveTwoPhone = user.RelativeTwoPhoneNumber,
                user.Gender,
                user.City,
            });
        }

        [HttpPut]
        [Route("api/PatientProfile/UpdatePatient/{PatientId}")]
        public async Task<IActionResult> UpdatePatient(string patientId, [FromBody] UpdatePatientModel model)
        {
            var user = await _userManager.FindByIdAsync(patientId);

            if (user == null)
                return BadRequest();

            user.City = model.City;
            user.RelativeOneName = model.RelativeOneName;
            user.RelativeOnePhoneNumber = model.RelativeOnePhoneNumber;
            user.RelativeTwoName = model.RelativeTwoName;
            user.RelativeTwoPhoneNumber = model.RelativeTwoPhoneNumber;
            user.PhoneNumber = model.PhoneNumber;

            await _userManager.UpdateAsync(user);
            return Ok();
        }
    }
}
