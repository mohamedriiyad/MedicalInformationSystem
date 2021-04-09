﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalInformationSystem.Models
{
    public class HospitalModel
    {

        
        [Key]
        public int Id { get; set; }
       
        public string Name { get; set; }

        public string Location { get; set; }
    

        public string Email { get; set; }
        
        public string Password { get; set; }
        public string File { get; set; }


    }
}
