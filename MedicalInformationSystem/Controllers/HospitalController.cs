using MedicalInformationSystem.Models;
using MedicalInformationSystem.Persistant;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace MedicalInformationSystem.Controllers
{
    public class HospitalController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MedicalSystemDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;


        public HospitalController(UserManager<ApplicationUser> userManager, MedicalSystemDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;

        }


        [HttpPost]
        [Route("api/ApplicationUser/posthospital")]

        public async Task<IActionResult> PostUser([FromBody] HospitalSignUpModel model) // from from in frontend //
        {


            var x = await _roleManager.RoleExistsAsync("Patient");
            if (!x)
            {
                var role = new IdentityRole { Name = "Patient" };
                await _roleManager.CreateAsync(role);
            }


            var y = await _roleManager.RoleExistsAsync("Admin");
            if (!y)
            {
                var role = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(role);
            }

            var z = await _roleManager.RoleExistsAsync("hospital");
            if (!z)
            {
                var role = new IdentityRole { Name = "hospital" };
                await _roleManager.CreateAsync(role);
            }

            if (!ModelState.IsValid) 
                return StatusCode(201);
            
            var hospital = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(hospital, model.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }

            await _userManager.AddToRoleAsync(hospital, "hospital");
            var files = model.Files.Select(file => new HospitalFile { FilePath = file.FilePath, HospitalModelId = file.HospitalId }).ToList();

            var hospitalInDb = new HospitalModel
            {
                Email = model.Email,
                ApplicationUserId = hospital.Id,
                Files = files,
                Location = model.Location,
                Name = model.Name,
                Password = model.Password
            };
            await _context.Hospitals.AddAsync(hospitalInDb);
            await _context.SaveChangesAsync();

            return Ok(result);
        }


        [HttpPost]
        [Route("api/ApplicationUser/LoginHospital")]

        public async Task<IActionResult> Login([FromBody] HospitalLogin model)
        {
            var user = _userManager.Users.FirstOrDefault(e => e.Email == model.Email);
            var hospital = _context.Hospitals.FirstOrDefault(e => e.Email == model.Email);
            
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return BadRequest(new {message = "patientSSN or password is incorrect."});
            
            var role = await _userManager.GetRolesAsync(user);
            IdentityOptions options = new IdentityOptions();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:44309/",
                audience: "https://localhost:44309/",
                claims: new List<Claim>()
                {
                    new Claim(options.ClaimsIdentity.RoleClaimType , role.FirstOrDefault())
                },
                expires: DateTime.Now.AddDays(1),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            if (hospital != null)
                return Ok(new {Token = tokenString, hospital.Id});

            return BadRequest("there is an inner exception");
        }

        [HttpGet]
        [Route("api/Hospital/getHospitalById/{id}")]
        public async Task<IActionResult> GetHospital(int id)
        {
            var hospital = await _context.Hospitals.FindAsync(id);
            if (hospital == null)
                return BadRequest();

            return Ok( new
                {
                    hospital.Id,
                    hospital.Name,
                    hospital.Location,
                    hospital.Email
                });
        }
    }
}
