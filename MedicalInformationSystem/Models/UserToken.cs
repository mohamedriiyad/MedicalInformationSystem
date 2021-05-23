using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
