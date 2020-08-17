using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CheckNote.Shared.Models
{
    public class User : IdentityUser<int>
    {
        public List<Note> Notes { get; }

        public static implicit operator UserOutput(User user)
        {
            return new UserOutput
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }

    public class UserInput : UserOutput
    {
        public static implicit operator User(UserInput input)
        {
            return new User
            {
                Id = input.Id,
                UserName = input.UserName,
                Email = input.Email
            };
        }
    }

    public class UserOutput
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
