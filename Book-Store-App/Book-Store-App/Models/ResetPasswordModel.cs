using System.ComponentModel.DataAnnotations;

namespace Book_Store_App.Models
{
    public class ResetPasswordModel
    {
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

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
