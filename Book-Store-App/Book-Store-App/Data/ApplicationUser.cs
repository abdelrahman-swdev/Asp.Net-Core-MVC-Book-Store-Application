using Microsoft.AspNetCore.Identity;

namespace Book_Store_App.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
