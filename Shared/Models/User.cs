using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CheckNote.Shared.Models
{
    public class User : IdentityUser<int>
    {
        public List<Note> Notes { get; }
    }
}
