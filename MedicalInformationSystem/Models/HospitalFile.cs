using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class HospitalFile
    {
        [Key]
        public int Id { get; set; }

        public string FilePath { get; set; }

        public HospitalModel HospitalModel { get; set; }
        public int? HospitalModelId { get; set; }
        public HospitalConfirmation HospitalConfirmation { get; set; }
        public int? HospitalConfirmationId { get; set; }
    }
}
