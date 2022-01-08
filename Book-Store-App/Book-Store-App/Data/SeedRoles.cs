using Book_Store_App.Enums;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Data
{
    public static class SeedRoles
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
                await roleManager.CreateAsync(new IdentityRole { Name = Roles.Basic.ToString() });
            }
        }
    }
}
