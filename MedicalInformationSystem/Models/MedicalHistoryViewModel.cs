using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class MedicalHistoryViewModel
    {
        public int Id { get; set; }
        public string BloodType { get; set; }
        public string PatientSSN { get; set; }
        public string FullName { get; set; }
        public string ApplicationUserId { get; set; }
        public ICollection<Operation> Operations { get; set; }
        public ICollection<Disease> Diseases { get; set; }
        public ICollection<Sensitivity> Sensitivities { get; set; }
    }

    public class OperationViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int MedicalHistoryId { get; set; }
    }

    public class DiseaseViewModel
    {
        public string Name { get; set; }
        public int MedicalHistoryId { get; set; }
    }

    public class SensitivityViewModel
    {
        public string Name { get; set; }
        public int MedicalHistoryId { get; set; }
    }
}
