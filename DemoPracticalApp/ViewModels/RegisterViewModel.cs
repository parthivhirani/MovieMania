using System.ComponentModel.DataAnnotations;

namespace DemoPracticalApp.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
