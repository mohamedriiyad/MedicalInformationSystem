using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MedicalInformationSystem.Models
{
    public class HospitalUploadModel
    {
        public IFormFileCollection Files { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
