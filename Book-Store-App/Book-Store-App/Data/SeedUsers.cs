using Book_Store_App.Enums;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Book_Store_App.Data
{
    public static class SeedUsers
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            var user = new ApplicationUser
            {
                FirstName = "site",
                LastName = "admin",
                Email = "admin@test.com",
                UserName = "admin@test.com",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
