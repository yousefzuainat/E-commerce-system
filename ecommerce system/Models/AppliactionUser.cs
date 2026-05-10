using Microsoft.AspNetCore.Identity;

namespace ecommerce_system.Models
{
    public class AppliactionUser : IdentityUser
    {
        public string FullName { get; set; }

    }

}
