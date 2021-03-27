using MedicalInformationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Controllers
{
    public class ApplicationUserController:Controller
    {
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly RoleManager<IdentityRole> rolemanager;
        public ApplicationUserController(UserManager<ApplicationUser> usermanager, RoleManager<IdentityRole> rolemanager)
        {
            this.usermanager = usermanager;
            this.rolemanager = rolemanager;
            
        }



        [HttpPost]
        [Route("api/ApplicationUser/postUser")]

        public async Task<IActionResult> postUser([FromBody] ApplicationUserModel model) // from from in frontend //
        {

            if (ModelState.IsValid)
            {


                bool x = await this.rolemanager.RoleExistsAsync("Patient");
                if (!x)
                {
                    var role = new IdentityRole();
                    role.Name = "Patient";
                    await rolemanager.CreateAsync(role);

                }


                bool y = await this.rolemanager.RoleExistsAsync("Admin");
                if (!y)
                {
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    await rolemanager.CreateAsync(role);

                }

                bool z = await this.rolemanager.RoleExistsAsync("hospital");
                if (!y)
                {
                    var role = new IdentityRole();
                    role.Name = "hospital";
                    await rolemanager.CreateAsync(role);

                }


                model.Role = "Admin";

                var User = new ApplicationUser()
                {

                    FullName = model.FullName,
                    PatientSSN = model.PatientSSN,
                    UserName = model.PatientSSN,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    RelativeOneName = model.RelativeOneName,
                    RelativeOnePhoneNumber = model.RelativeOnePhoneNumber,
                    RelativeTwoName = model.RelativeTwoName,
                    RelativeTwoPhoneNumber = model.RelativeTwoPhoneNumber,
                    City = model.City
                };
                var result = await this.usermanager.CreateAsync(User, model.Password);

                if (result.Succeeded)
                {
                    var addRole = await this.usermanager.AddToRoleAsync(User, model.Role);
                    return Ok(result);
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description);
                    // var message = "Invalied social security Number";
                    return BadRequest(errors);
                }
            }


            return StatusCode(201);

        }










        [HttpPost]
        [Route("api/ApplicationUser/Login")]

        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {


            var user = usermanager.Users.FirstOrDefault(e => e.PatientSSN == model.PatientSSN);
            if (user != null && await usermanager.CheckPasswordAsync(user, model.Password)) // valid user //
            {
                var role = await usermanager.GetRolesAsync(user);

                IdentityOptions _options = new IdentityOptions();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:4200",
                audience: "http://localhost:4200",
                 claims: new List<Claim>() {
                new Claim(_options.ClaimsIdentity.RoleClaimType , role.FirstOrDefault())

                 },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials

               ); 

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, UserId = user.Id });


            }


            else
            {

                return BadRequest(new { message = "patientSSN or password is incorrect." });
            }




        }





















    }
}
