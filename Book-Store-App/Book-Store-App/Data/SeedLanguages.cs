using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_App.Data
{
    public static class SeedLanguages
    {
        public static async Task SeedInitialLanguages(ApplicationDbContext context)
        {
            if(!context.Languages.Any())
            {
                await context.Languages.AddAsync(new Language() { LanguageName = "English" });
                await context.Languages.AddAsync(new Language() { LanguageName = "Spanish" });
                await context.Languages.AddAsync(new Language() { LanguageName = "عربي" });
                await context.SaveChangesAsync();
            }
        }
    }
}
