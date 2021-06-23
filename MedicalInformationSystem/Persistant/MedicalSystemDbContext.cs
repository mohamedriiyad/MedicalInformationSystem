using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MedicalInformationSystem.Models;

namespace MedicalInformationSystem.Persistant
{
    public class MedicalSystemDbContext :IdentityDbContext
    {
        public MedicalSystemDbContext(DbContextOptions<MedicalSystemDbContext> options) 
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Patients { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<Operation> Operations  { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Sensitivity> Sensitivities { get; set; }
        public DbSet<HospitalModel> Hospitals { get; set; }
        public DbSet<HospitalFile> HospitalFiles { get; set; }
        public DbSet<HospitalConfirmation> HospitalConfirmations { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserToken> Tokens { get; set; }
        public DbSet<NotificationModel> BloodDonations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<HospitalFile>()
                .HasOne(f => f.HospitalModel)
                .WithMany(m => m.Files)
                .HasForeignKey(f => f.HospitalModelId);

            base.OnModelCreating(builder);
        }
    }
}
