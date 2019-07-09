using System;
using System.ComponentModel.DataAnnotations;

namespace Everest.AspNetStartup.Models
{
    public class AddUserModel
    {
        [Required]
        [MinLength(length: 3)]
        public string Name { get; set; }

        [Required]
        [MinLength(length: 3)]
        public string Surname { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
