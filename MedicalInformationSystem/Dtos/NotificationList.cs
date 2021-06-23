using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Dtos
{
    public class NotificationList
    {
        public string PatientName { get; set; }
        public string BloodType { get; set; }
        public string NumberOfBags { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public HospitalDto Hospital { get; set; }
    }
}
