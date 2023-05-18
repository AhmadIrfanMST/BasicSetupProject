using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class MyDbContext : IdentityDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Pie> pies { get; set; }
        public DbSet<ApplicationUser> aspnetusers { get; set; }
        public DbSet<Permission> permissions { get; set; }
    }
}
