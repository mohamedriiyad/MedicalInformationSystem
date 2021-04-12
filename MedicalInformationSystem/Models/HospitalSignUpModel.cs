using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class HospitalSignUpModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<HospitalFileModel> Files { get; set; }
    }
}
