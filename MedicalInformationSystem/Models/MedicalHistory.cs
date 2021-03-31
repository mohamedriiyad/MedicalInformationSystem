using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        public string BloodType { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Sensitivity> Sensitivities { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
        public virtual ICollection<Operation> Operations { get; set; }
    }
}
