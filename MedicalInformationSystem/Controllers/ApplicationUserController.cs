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
        private readonly MedicalSystemDbContext _context;
        public ApplicationUserController(UserManager<ApplicationUser> userManager, MedicalSystemDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Route("api/ApplicationUser/postUser")]
        public async Task<IActionResult> PostUser([FromBody] ApplicationUserModel model) // from from in frontend //
        {
            if (!ModelState.IsValid)
                return StatusCode(201);
            
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
                await _userManager.AddToRoleAsync(user, "Patient");
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
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(new {message = "patientSSN or password is incorrect."});

            var roles = await _userManager.GetRolesAsync(user);
                
            var options = new IdentityOptions();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokeOptions;
            if (roles.Count == 0)
            {
                tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44309/",
                    audience: "https://localhost:44309/",
                    claims: new List<Claim>() {
                        new Claim(options.ClaimsIdentity.RoleClaimType , "Patient")
                    },
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signinCredentials
                );
            }
            else
            {
                tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44309/",
                    audience: "https://localhost:44309/",
                    claims: new List<Claim>() {
                        new Claim(options.ClaimsIdentity.RoleClaimType , roles.FirstOrDefault())
                    },
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signinCredentials
                );
            }

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

        private bool TokenExists(string token, string id)
        {
            return _context.Tokens.Any(t => t.Token == token && t.ApplicationUserId == id); 
        }

    }
}
