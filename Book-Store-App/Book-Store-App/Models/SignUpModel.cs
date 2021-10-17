using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [MaxLength(256)]
        public string LastName { get; set; }


        [Required(ErrorMessage ="Email is required")]
        [Display(Name ="Email Address")]
        [EmailAddress(ErrorMessage ="Email is not valid")]
        [MaxLength(256)]
        public string Email  {get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password,ErrorMessage ="Password must be strong")]
        [MinLength(8,ErrorMessage ="Password should at least have 8 characters")]
        public string Password  {get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Password doesn't match")]
        [Compare("Password",ErrorMessage = "Password doesn't match")]
        [Display(Name ="Confirm your password")]
        public string ConfirmPassword  {get; set; }
    }
}
