using Microsoft.AspNetCore.Identity;

namespace WebApplication4IdentityDb.Models
{
    public class AppUser : IdentityUser
    {

        public string BarCode { get; set; }
    }
}
