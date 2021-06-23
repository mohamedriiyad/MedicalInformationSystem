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
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }



        [HttpPost]
        [Route("api/ApplicationUser/postUser")]
        public async Task<IActionResult> PostUser([FromBody] ApplicationUserModel model) // from from in frontend //
        {
            if (!ModelState.IsValid)
                return StatusCode(201);
            var x = await _roleManager.RoleExistsAsync("Patient");
            if (!x)
            {
                var role = new IdentityRole {Name = "Patient"};
                await _roleManager.CreateAsync(role);
            }


            var y = await _roleManager.RoleExistsAsync("Admin");
            if (!y)
            {
                var role = new IdentityRole {Name = "Admin"};
                await _roleManager.CreateAsync(role);
            }

            var z = await _roleManager.RoleExistsAsync("hospital");
            if (!z)
            {
                var role = new IdentityRole {Name = "hospital"};
                await _roleManager.CreateAsync(role);
            }


            model.Role = "Admin";

            var user = new ApplicationUser()
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
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
                return Ok(result);
            }

            var errors = result.Errors.Select(e => e.Description);
            return BadRequest(errors);


        }


        [HttpPost]
        [Route("api/ApplicationUser/Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = _userManager.Users.FirstOrDefault(e => e.PatientSSN == model.PatientSSN);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) // valid user //
            {
                var role = await _userManager.GetRolesAsync(user);
                
                IdentityOptions options = new IdentityOptions();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:44309/",
                audience: "https://localhost:44309/",
                claims: new List<Claim>() {
                new Claim(options.ClaimsIdentity.RoleClaimType , role.FirstOrDefault())

                 },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials

               ); 

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                if (TokenExists(model.DeviceToken, user.Id)) 
                    return Ok(new {Token = tokenString, UserId = user.Id});
                
                // Store token
                var token = _context.Tokens.SingleOrDefault(t => t.ApplicationUserId == user.Id);
                if (token == null)
                {
                    _context.Tokens.Add(new UserToken
                    {
                        ApplicationUserId = user.Id,
                        CreationDate = DateTime.Now,
                        Token = model.DeviceToken
                    });
                }
                else
                {
                    token.Token = model.DeviceToken;
                    token.CreationDate = DateTime.Now;
                    _context.Tokens.Update(token);
                }

                _context.SaveChanges();
                return Ok(new { Token = tokenString, UserId = user.Id });
            }

            return BadRequest(new { message = "patientSSN or password is incorrect." });
        }

        private bool TokenExists(string token, string id)
        {
            return _context.Tokens.Any(t => t.Token == token && t.ApplicationUserId == id); 
        }

    }
}
