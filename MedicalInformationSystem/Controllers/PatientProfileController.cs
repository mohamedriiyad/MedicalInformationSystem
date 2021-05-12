using MedicalInformationSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Controllers
{
    public class PatientProfileController:Controller
    {

        private readonly UserManager<ApplicationUser> usermanager;
        public PatientProfileController(UserManager<ApplicationUser> usermanager)
        {

            this.usermanager = usermanager;
        }


        /*

       [HttpGet]
       [Authorize]
       //GET : /api/UserProfile

       public async Task<Object> GetUserProfile()
       {
           string userId = User.Claims.First(c => c.Type == "UserID").Value;
           var user = await usermanager.FindByIdAsync(userId);
           return new
           {
               user.FullName,
               user.Email,
               user.UserName,
               user.City,
               user.Gender
           };
       }
       */

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
        public async Task<IActionResult> getPatientById(string PatientId)
        {
            var user = await this.usermanager.FindByIdAsync(PatientId);
            if (user == null)
            {
                return BadRequest("sorry user not found");
            }
            return Ok(new
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender,
                City = user.City,
                DateOfBirth = user.DateOfBirth,
                RealtiveOneName = user.RelativeOneName,
                RealtiveTwoName = user.RelativeTwoName,
                RealtiveOnePhone = user.RelativeOnePhoneNumber,
                RealtiveTwoPhone = user.RelativeTwoPhoneNumber,
            });
        }


        [HttpGet]
        [Route("api/PatientProfile/getPatientBySSN/{PatientSSN}")]
       // [Authorize]

        public async Task<IActionResult> getPatientBySSN(string PatientSSN)
        {
            var user = await this.usermanager.Users.FirstAsync(e => e.PatientSSN == PatientSSN);
            if (user == null)
            {
                return BadRequest("sorry user not found");
            }
            return Ok(new
            {
                Id =  user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                RealtiveOneName = user.RelativeOneName,
                RealtiveTwoName = user.RelativeTwoName,
                RealtiveOnePhone = user.RelativeOnePhoneNumber,
                RealtiveTwoPhone = user.RelativeTwoPhoneNumber,
                Gender = user.Gender,
                City = user.City,

            });
        }



        [HttpPut]
        [Route("api/PatientProfile/UpdatePatient/{PatientId}")]
        public async Task<IActionResult> UpdatePatient(string PatientId, [FromBody] UpdatePatientModel model)
        {
            var user = await this.usermanager.FindByIdAsync(PatientId);

            if (user == null)
            {
                return BadRequest();
            }


            user.City = model.City;
            user.RelativeOneName = model.RelativeOneName;
            user.RelativeOnePhoneNumber = model.RelativeOnePhoneNumber;
            user.RelativeTwoName = model.RelativeTwoName;
            user.RelativeTwoPhoneNumber = model.RelativeTwoPhoneNumber;
            user.PhoneNumber = model.PhoneNumber;


            await this.usermanager.UpdateAsync(user);
            return Ok();



        }













    }
}
