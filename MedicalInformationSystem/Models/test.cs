using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class Test
    {
        public int  Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public string Image { get; set; }
        public int MedicalHistoryId { get; set; }

    }
}
