using Microsoft.EntityFrameworkCore;
using Book_Store_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Book_Store_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Gallery> Gallery { get; set; }
        public DbSet<Language> Languages { get; set; }
    }
}
