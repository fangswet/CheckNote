using Microsoft.AspNetCore.Identity;

namespace CheckNote.Shared.Models
{
    public class Role : IdentityRole<int>
    {
        public static readonly string Admin = "admin";
    }
}
