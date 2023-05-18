using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string CardId { get; set; }
        public string Discriminator { get; set; }   
    }
}
