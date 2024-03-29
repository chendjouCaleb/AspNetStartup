﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Everest.AspNetStartup.Models
{
    public class UserInfo
    {
        [Required]
        [MinLength(length: 3)]
        public string Name { get; set; }

        [Required]
        [MinLength(length: 3)]
        public string Surname { get; set; }

        
        public DateTime BirthDate { get; set; }

        [StringLength(maximumLength:1, MinimumLength = 1)]
        public string Gender { get; set; }

 
        public string NationalId { get; set; }
    }
}
