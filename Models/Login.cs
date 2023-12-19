using System.ComponentModel.DataAnnotations;

namespace Task4Attempt2.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        
    }
}
