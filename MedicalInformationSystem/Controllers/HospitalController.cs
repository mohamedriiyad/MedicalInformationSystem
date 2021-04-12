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
using System.IO;
using MedicalInformationSystem.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MedicalInformationSystem.Controllers
{
    public class HospitalController:Controller
    {

        private readonly UserManager<ApplicationUser> usermanager;
        private readonly MedicalSystemDbContext context;
        private readonly RoleManager<IdentityRole> rolemanager;


        public HospitalController(UserManager<ApplicationUser> usermanager , MedicalSystemDbContext context , RoleManager<IdentityRole> rolemanager)
        {

            this.usermanager = usermanager;
            this.context = context;
            this.rolemanager = rolemanager;



        }


        [HttpPost]
        [Route("api/ApplicationUser/posthospital")]

        public async Task<IActionResult> PostUser([FromBody] HospitalSignUpModel model) // from from in frontend //
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




            if (ModelState.IsValid)
            {


                var hospital = new ApplicationUser()
                {
                    Email = model.Email,
                    UserName = model.Email,


                };


                var result = await usermanager.CreateAsync(hospital, model.Password);
             
                if (result.Succeeded)
                {
                    await usermanager.AddToRoleAsync(hospital, "hospital");
                    var files = model.Files.Select(file => new HospitalFile {FilePath = file.FilePath, HospitalModelId = file.HospitalId}).ToList();
                   
                    var hospitalInDb = new HospitalModel
                    {
                        Email = model.Email,
                        ApplicationUserId = hospital.Id,
                        Files = files,
                        Location = model.Location,
                        Name = model.Name,
                        Password = model.Password
                    };
                    await context.Hospitals.AddAsync(hospitalInDb);
                    await context.SaveChangesAsync();

                    
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.HospitalFiles ON");
                    //context.SaveChanges();
                    //await context.HospitalFiles.AddRangeAsync(model.Files);
                    //await context.SaveChangesAsync();
                    
                    //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.HospitalFiles OFF");
                    //context.SaveChanges();

                    return Ok(result);
                }

                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(errors);
            }


            return StatusCode(201);

        }


        [HttpPost]
        [Route("api/ApplicationUser/Loginhospital")]

        public async Task<IActionResult> Login([FromBody] HospitalLogin model)
        {


            var user = usermanager.Users.FirstOrDefault(e => e.Email == model.Email);
            if (user != null && await usermanager.CheckPasswordAsync(user, model.Password)) // valid user //
            {
                var role = await usermanager.GetRolesAsync(user);



                IdentityOptions _options = new IdentityOptions();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:44309/",
                audience: "https://localhost:44309/",
                 claims: new List<Claim>()
                 {
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


        [HttpPost, DisableRequestSizeLimit]
        [Route("api/ApplicationUser/UploadFiles")]
        public IActionResult UploadFiles()
        {
            try
            {
                var files = Request.Form.Files.ToList();
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (files.Any(f => f.Length == 0))
                    return BadRequest("There is no files or empty files");

                FileHelper.UploadAll(files,pathToSave);
                return Ok("All files are UPLOADED SUCCESSFULLY!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }




        /*
          [Route("api/ApplicationUser/gethospitaldata/{id}")]
          public async Task<IActionResult> gethospitaldata(int id)
          {
              var hospital = await this.context.Hospitals.FindAsync(id);
              return Ok(hospital);


          }
        */





    }
}
