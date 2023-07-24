using Microsoft.AspNetCore.Identity;

namespace MagicVilla_WebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
