using System.ComponentModel.DataAnnotations;

namespace Book_Store_App.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password is wrong")]
        [MinLength(8, ErrorMessage = "Password is wrong")]
        [Display(Name = "New password")]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password must be strong")]
        [MinLength(8, ErrorMessage = "Password should at least have 8 characters")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password")]
        [DataType(DataType.Password, ErrorMessage = "Password doesn't match")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
