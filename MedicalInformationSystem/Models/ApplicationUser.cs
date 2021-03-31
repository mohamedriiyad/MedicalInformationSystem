using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class ApplicationUser: IdentityUser
    {

        [Required]

        public string PatientSSN { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3,
        ErrorMessage = "Full Name should be minimum 3 characters and a maximum of 100 characters")]
        [DataType(DataType.Text)]
        public string FullName { get; set; }


        [Required]
        public string City { get; set; }

        [Required]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string RelativeOneName { get; set; }
        [Required]
        public string RelativeOnePhoneNumber { get; set; }
        [Required]
        public string RelativeTwoName { get; set; }
        [Required]
        public string RelativeTwoPhoneNumber { get; set; }

        public virtual MedicalHistory MedicalHistory { get; set; }
        



        public ApplicationUser()
        {

        }

    }
}
