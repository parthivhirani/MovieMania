using System.ComponentModel.DataAnnotations;

namespace DemoPractical.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter valid email address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
