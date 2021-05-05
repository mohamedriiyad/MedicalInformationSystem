using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MedicalInformationSystem.Models
{
    public class TestUploadModel
    {
        public IFormFileCollection Files { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
    }
}
