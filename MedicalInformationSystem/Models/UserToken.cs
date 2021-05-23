using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
