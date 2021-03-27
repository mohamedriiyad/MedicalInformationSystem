using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class ApplicationUserModel
    {

        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PatientSSN { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string userName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string RelativeOneName { get; set; }
        public string RelativeOnePhoneNumber { get; set; }
        public string RelativeTwoName { get; set; }
        public string RelativeTwoPhoneNumber { get; set; }

        public string Role { get; set; }




    }
}
