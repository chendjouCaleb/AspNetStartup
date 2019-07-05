using System.ComponentModel.DataAnnotations;

namespace Everest.AspNetStartup.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Browser { get; set; }

        [Required]
        public string OS { get; set; }

        public string RemoteAddress { get; set; }

        public bool IsPersisted { get; set; }
    }
}
