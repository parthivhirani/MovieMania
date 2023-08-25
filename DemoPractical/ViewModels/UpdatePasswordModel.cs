using System.ComponentModel.DataAnnotations;

namespace DemoPractical.ViewModels
{
    public class UpdatePasswordModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get;set; }
    }
}
