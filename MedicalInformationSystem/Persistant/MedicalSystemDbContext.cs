using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalInformationSystem.Models;

namespace MedicalInformationSystem.Persistant
{
    public class MedicalSystemDbContext :IdentityDbContext
    {


        public MedicalSystemDbContext(DbContextOptions<MedicalSystemDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> Patients { get; set; }
        public DbSet<MedicalHistory> MedicalHistory { get; set; }
        public DbSet<Operation> Operations  { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Sensitivity> Sensitivities { get; set; }
        public DbSet<HospitalModel> hospitals { get; set; }


    }
}
