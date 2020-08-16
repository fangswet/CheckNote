using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class User : IdentityUser<int>
    {
        public List<Note> Notes { get; }

        [NotMapped]
        public string Jwt { get; set; }
    }
}
