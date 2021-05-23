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
using MedicalInformationSystem.Persistant;

namespace MedicalInformationSystem.Controllers
{
    public class ApplicationUserController:Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MedicalSystemDbContext _context;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, MedicalSystemDbContext context)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            _context = context;
        }



        [HttpPost]
        [Route("api/ApplicationUser/postUser")]
        public async Task<IActionResult> postUser([FromBody] ApplicationUserModel model) // from from in frontend //
        {

            if (ModelState.IsValid)
            {
                bool x = await this._roleManager.RoleExistsAsync("Patient");
                if (!x)
                {
                    var role = new IdentityRole();
                    role.Name = "Patient";
                    await _roleManager.CreateAsync(role);

                }


                bool y = await this._roleManager.RoleExistsAsync("Admin");
                if (!y)
                {
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    await _roleManager.CreateAsync(role);

                }

                bool z = await this._roleManager.RoleExistsAsync("hospital");
                if (!y)
                {
                    var role = new IdentityRole();
                    role.Name = "hospital";
                    await _roleManager.CreateAsync(role);

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
                var result = await this._userManager.CreateAsync(User, model.Password);

                if (result.Succeeded)
                {
                    var addRole = await this._userManager.AddToRoleAsync(User, model.Role);
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
            var user = _userManager.Users.FirstOrDefault(e => e.PatientSSN == model.PatientSSN);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) // valid user //
            {
                var role = await _userManager.GetRolesAsync(user);
                
                IdentityOptions _options = new IdentityOptions();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:44309/",
                audience: "https://localhost:44309/",
                 claims: new List<Claim>() {
                new Claim(_options.ClaimsIdentity.RoleClaimType , role.FirstOrDefault())

                 },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials

               ); 

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                
                // Store token
                _context.Tokens.Add(new UserToken
                {
                    ApplicationUserId = user.Id,
                    CreationDate = DateTime.Now,
                    Token = tokenString
                });
                _context.SaveChanges();

                return Ok(new { Token = tokenString, UserId = user.Id });
            }

            return BadRequest(new { message = "patientSSN or password is incorrect." });
        }

    }
}
