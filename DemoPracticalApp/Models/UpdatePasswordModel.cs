using DemoPracticalApp.CustomAttributes;
using System.ComponentModel.DataAnnotations;

namespace DemoPracticalApp.Models
{
    public class UpdatePasswordModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [NotEqual("CurrentPassword", ErrorMessage = "Current password and new password can't be same")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't matched!")]
        public string ConfirmNewPassword { get; set; }
    }
}
