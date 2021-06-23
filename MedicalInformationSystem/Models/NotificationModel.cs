using System;

namespace MedicalInformationSystem.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string NumberOfBags { get; set; }
        public string BloodType { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public HospitalModel HospitalModel { get; set; }
        public int HospitalModelId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
